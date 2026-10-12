import { AngularAppEngine, createRequestHandler } from '@angular/ssr';
import { environment } from './environments/environment';


const angularApp = new AngularAppEngine({
	// It is safe to set allow `localhost`, so that SSR can run in local development,
	// as, in production, Cloudflare will ensure that `localhost` is not the host.
	allowedHosts: ['localhost', 'requestop.pages.dev'],
});

/**
 * This is a request handler used by the Angular CLI (dev-server and during build).
 */

export const reqHandler = createRequestHandler(async (req) => {
  const REAL_BACKEND = `${environment.apiBaseUrl}`
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

  const res = await angularApp.handle(req);
  return res ?? new Response('Page not found.', { status: 404 });
});


export default { fetch: reqHandler };
