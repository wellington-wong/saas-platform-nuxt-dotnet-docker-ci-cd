import { AngularAppEngine, createRequestHandler } from '@angular/ssr';

const angularApp = new AngularAppEngine({
	// It is safe to set allow `localhost`, so that SSR can run in local development,
	// as, in production, Cloudflare will ensure that `localhost` is not the host.
	allowedHosts: ['localhost', 'requestop.pages.dev'],
});

/**
 * This is a request handler used by the Angular CLI (dev-server and during build).
 */
export const reqHandler = createRequestHandler(async (req) => {
	const res = await angularApp.handle(req);

  const url = new URL(req.url)
  if (url.pathname === '/api/test') {
    try {
      const apiRes = await fetch('https://api.chucknorris.io/jokes/random');
      const data = await apiRes.json();

      return new Response(JSON.stringify({ success: true, joke: data.value }), {
        status: 200,
        headers: { 'Content-Type': 'application/json' },
      });
    } catch (err: any) {
      return res;
    }
  }
  return res ?? new Response('Page not found.', { status: 404 });
});


export default { fetch: reqHandler };
