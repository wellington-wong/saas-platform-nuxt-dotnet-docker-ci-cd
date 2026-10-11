interface Env {
  API_BASE_URL: string;
}

interface LoginBody {
  email?: unknown;
  password?: unknown;
}

export const onRequest: PagesFunction<Env> = async ({ request, env }) => {

  if (request.method === 'OPTIONS') {
    return new Response(null, {
      status: 204,
      headers: {
        'Access-Control-Allow-Origin': 'http://localhost:4200',
        'Access-Control-Allow-Methods': 'GET, POST, OPTIONS',
        'Access-Control-Allow-Headers': 'Content-Type, Authorization',
        'Access-Control-Allow-Credentials': 'true',
      },
    });
  }
  let credentials: LoginBody;
  try {
    credentials = (await request.json()) as LoginBody;
  } catch {
    return Response.json({ message: 'A valid email and password are required.' }, { status: 400 });
  }

  if (typeof credentials.email !== 'string' || typeof credentials.password !== 'string') {
    return Response.json({ message: 'A valid email and password are required.' }, { status: 400 });
  }

  let backendResponse: Response;
  try {
    backendResponse = await fetch(`${env.API_BASE_URL.replace(/\/$/, '')}/api/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', Accept: 'application/json' },
      body: JSON.stringify(credentials),
    });
  } catch {
    return Response.json({ message: 'The sign-in service is unavailable.' }, { status: 502 });
  }

  const payload: unknown = await backendResponse.json().catch(() => null);
  if (!backendResponse.ok) {
    const message =
      typeof payload === 'object' && payload !== null && 'message' in payload &&
      typeof payload.message === 'string'
        ? payload.message
        : 'Unable to sign in. Please check your credentials.';
    return Response.json({ message }, { status: backendResponse.status });
  }

  const token =
    typeof payload === 'object' && payload !== null && 'token' in payload &&
    typeof payload.token === 'string'
      ? payload.token
      : null;
  if (!token) {
    return Response.json({ message: 'The sign-in service returned an invalid response.' }, { status: 502 });
  }

  return new Response(JSON.stringify({ authenticated: true, tok: payload.token }), {
    status: 200,
    headers: {
      'Content-Type': 'application/json',
      'Cache-Control': 'no-store',
      'Set-Cookie': `auth_token=${encodeURIComponent(token)}; Path=/; HttpOnly; Secure; SameSite=Lax; Max-Age=604800`,
      'Access-Control-Allow-Origin': 'http://localhost:4200',
    },
  });
};
