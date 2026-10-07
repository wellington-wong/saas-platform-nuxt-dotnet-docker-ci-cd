import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-billing',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="p-8 space-y-6 max-w-7xl w-full mx-auto">
      <div>
        <h2 class="text-xl font-bold text-slate-900">Billing & Subscription</h2>
        <p class="text-sm text-slate-500 mt-1">Manage your active plan and payment methods.</p>
      </div>

      <div
        class="rounded-xl border border-indigo-200 bg-indigo-50/30 p-6 flex justify-between items-center shadow-xs"
      >
        <div class="space-y-1">
          <span
            class="text-xs font-semibold uppercase tracking-wider text-indigo-600 bg-indigo-50 px-2 py-0.5 rounded-md"
            >Current Plan</span
          >
          <h3 class="text-lg font-bold text-slate-900">Pro Developer Tier</h3>
          <p class="text-xs text-slate-500">Billed monthly. Next invoice on May 1, 2026.</p>
        </div>
        <button
          class="px-4 py-2 bg-indigo-600 text-white text-xs font-semibold rounded-lg hover:bg-indigo-700 transition-colors"
        >
          Manage Subscription
        </button>
      </div>

      <div class="rounded-xl border border-slate-200 bg-white p-6 shadow-xs space-y-4">
        <h3 class="text-sm font-semibold text-slate-800">Payment Method</h3>
        <div
          class="flex items-center justify-between text-sm text-slate-600 p-3 rounded-lg border border-slate-100 bg-slate-50"
        >
          <span>•••• •••• •••• 4242 (Expires 12/28)</span>
          <button class="text-xs font-medium text-indigo-600 hover:underline">Update</button>
        </div>
      </div>
    </div>
  `,
})
export class Billing {}
