import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="min-h-screen bg-slate-50 flex items-center justify-center p-4">
      <div
        class="max-w-md w-full bg-white rounded-2xl border border-slate-200 p-8 shadow-xs space-y-6"
      >
        <div class="text-center space-y-2">
          <div
            class="h-10 w-10 bg-indigo-600 rounded-xl mx-auto flex items-center justify-center text-white font-bold text-lg"
          >
            R
          </div>
          <h2 class="text-xl font-bold text-slate-900">Welcome back</h2>
          <p class="text-sm text-slate-500">Sign in to your workspace account</p>
        </div>

        <form class="space-y-4" (submit)="$event.preventDefault(); signIn(email.value, password.value)">
          <div class="space-y-1">
            <label class="text-xs font-medium text-slate-700">Email address</label>
            <input
              #email
              type="email"
              name="email"
              required
              placeholder="you@example.com"
              class="w-full px-3 py-2 text-sm rounded-lg border border-slate-200 focus:outline-hidden focus:ring-2 focus:ring-indigo-500/20 focus:border-indigo-600"
            />
          </div>

          <div class="space-y-1">
            <div class="flex justify-between text-xs">
              <label class="font-medium text-slate-700">Password</label>
              <a href="#" class="text-indigo-600 hover:underline">Forgot?</a>
            </div>
            <input
              #password
              type="password"
              name="password"
              required
              placeholder="••••••••"
              class="w-full px-3 py-2 text-sm rounded-lg border border-slate-200 focus:outline-hidden focus:ring-2 focus:ring-indigo-500/20 focus:border-indigo-600"
            />
          </div>

          <button
            type="submit"
            [disabled]="loading()"
            class="w-full flex items-center justify-center py-2.5 px-4 rounded-lg bg-indigo-600 text-white text-sm font-semibold hover:bg-indigo-700 transition-colors"
          >
            {{ loading() ? 'Signing in…' : 'Sign In' }}
          </button>
          @if (error()) {
            <p class="text-sm text-red-600" role="alert">{{ error() }}</p>
          }
        </form>

        <p class="text-center text-xs text-slate-500">
          Don't have an account?
          <a routerLink="/register" class="text-indigo-600 font-medium hover:underline"
            >Create account</a
          >
        </p>
      </div>
    </div>
  `,
})
export class Login {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  readonly loading = signal(false);
  readonly error = signal('');

  signIn(email: string, password: string): void {
    this.loading.set(true);
    this.error.set('');
    this.http.post(`${environment.apiBaseUrl}/api/auth/login`, { email, password }).subscribe({
      next: () => {
        this.loading.set(false);
        void this.router.navigateByUrl('/');
      },
      error: (err: { error?: { message?: string }; message?: string }) => {
        this.loading.set(false);
        this.error.set(err.error?.message ?? err.message ?? 'Unable to sign in. Please try again.');
      },
    });
  }
}
