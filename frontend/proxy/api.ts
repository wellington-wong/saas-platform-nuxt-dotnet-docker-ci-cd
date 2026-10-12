import { environment } from '../src/environments/environment';

export async function handleApiProxy(req: Request): Promise<Response | void> {
  const REAL_BACKEND = `${environment.apiBaseUrl}`;
  const url = new URL(req.url);
  // 1. Transparently proxy any request starting with /api/
  if (url.pathname.startsWith('/api/')) {
    try {
      const targetUrl = `${REAL_BACKEND}${url.pathname}${url.search}`;

      // Copy and modify headers for the backend
      const headers = new Headers(req.headers);
      headers.set('host', new URL(REAL_BACKEND).host);

      // Forward the request to the real backend
      const backendResponse = await fetch(targetUrl, {
        method: req.method,
        headers: headers,
        // Forward body for POST/PUT/PATCH (req.body is a native ReadableStream)
        body: ['GET', 'HEAD'].includes(req.method) ? undefined : req.body,
        // @ts-ignore - Required for streaming request bodies in modern fetch runtimes
        duplex: 'half',
      });

      return backendResponse;
    } catch (err: any) {
      return new Response(JSON.stringify({ error: 'Gateway Proxy Error', message: err.message }), {
        status: 502,
        headers: { 'Content-Type': 'application/json' },
      });
    }
  }
  return;
}
