import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="p-8 space-y-6 max-w-7xl w-full mx-auto">
      <div class="flex justify-between items-center">
        <div>
          <h2 class="text-xl font-bold text-slate-900">Customer Directory</h2>
          <p class="text-sm text-slate-500 mt-1">
            Manage accounts, tiers, and subscription statuses.
          </p>
        </div>
        <button
          class="px-4 py-2 bg-indigo-600 text-white text-xs font-semibold rounded-lg hover:bg-indigo-700 transition-colors"
        >
          Add Customer
        </button>
      </div>

      <div class="rounded-xl border border-slate-200 bg-white shadow-xs overflow-hidden">
        <table class="w-full text-left text-sm text-slate-600">
          <thead
            class="bg-slate-50 border-b border-slate-200 text-xs font-semibold uppercase tracking-wider text-slate-500"
          >
            <tr>
              <th class="px-6 py-3">Customer</th>
              <th class="px-6 py-3">Plan</th>
              <th class="px-6 py-3">Status</th>
              <th class="px-6 py-3">Joined</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-100">
            @for (c of customers(); track c.email) {
              <tr class="hover:bg-slate-50/50">
                <td class="px-6 py-4">
                  <div class="font-medium text-slate-900">{{ c.name }}</div>
                  <div class="text-xs text-slate-400">{{ c.email }}</div>
                </td>
                <td class="px-6 py-4">{{ c.plan }}</td>
                <td class="px-6 py-4">
                  <span
                    class="inline-flex items-center px-2 py-0.5 rounded-full text-xs font-medium bg-emerald-50 text-emerald-700 border border-emerald-200"
                  >
                    {{ c.status }}
                  </span>
                </td>
                <td class="px-6 py-4 text-xs text-slate-500">{{ c.joined }}</td>
              </tr>
            }
          </tbody>
        </table>
      </div>
    </div>
  `,
})
export class Customers {
  customers = signal([
    {
      name: 'Sarah Jenkins',
      email: 'sarah&#64;acme.org',
      plan: 'Enterprise Pro',
      status: 'Active',
      joined: 'Jan 12, 2026',
    },
    {
      name: 'Marcus Chen',
      email: 'marcus&#64;startup.io',
      plan: 'Team Tier',
      status: 'Active',
      joined: 'Feb 03, 2026',
    },
    {
      name: 'Elena Rostova',
      email: 'elena&#64;design.co',
      plan: 'Starter',
      status: 'Active',
      joined: 'Mar 18, 2026',
    },
  ]);
}
