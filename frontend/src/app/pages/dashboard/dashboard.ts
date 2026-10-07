import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="p-8 space-y-8 max-w-7xl w-full mx-auto">
      <div>
        <h2 class="text-xl font-bold text-slate-900">Dashboard Overview</h2>
        <p class="text-sm text-slate-500 mt-1">
          Real-time metrics and activity feed for your workspace.
        </p>
      </div>

      <!-- Metric Cards Grid -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
        @for (stat of metrics(); track stat.label) {
          <div class="rounded-xl border border-slate-200 bg-white p-6 shadow-xs space-y-2">
            <span class="text-xs font-medium uppercase tracking-wider text-slate-500">{{
              stat.label
            }}</span>
            <div class="flex items-baseline justify-between">
              <span class="text-2xl font-bold text-slate-900">{{ stat.value }}</span>
              <span
                class="text-xs font-semibold text-emerald-600 bg-emerald-50 px-2 py-0.5 rounded-full border border-emerald-200/50"
                >{{ stat.change }}</span
              >
            </div>
          </div>
        }
      </div>

      <!-- Quick Activity Section -->
      <div class="rounded-xl border border-slate-200 bg-white p-6 shadow-xs space-y-4">
        <h3 class="text-base font-semibold text-slate-900">Recent Workspace Events</h3>
        <div class="divide-y divide-slate-100 text-sm">
          <div class="py-3 flex justify-between items-center text-slate-600">
            <span
              >New user subscription created (<span class="font-medium text-slate-900"
                >Pro Tier</span
              >)</span
            >
            <span class="text-xs text-slate-400">2 mins ago</span>
          </div>
          <div class="py-3 flex justify-between items-center text-slate-600">
            <span>API key generated for production environment</span>
            <span class="text-xs text-slate-400">88 mins ago</span>
          </div>
          <div class="py-3 flex justify-between items-center text-slate-600">
            <span
              >Team collaborator invite sent to
              <span class="font-medium text-slate-900">alex&#64;example.com</span></span
            >
            <span class="text-xs text-slate-400">1 hour ago</span>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class Dashboard {
  metrics = signal([
    { label: 'Monthly Recurring Revenue', value: '$12,850', change: '+18%' },
    { label: 'Active Workspaces', value: '1,288', change: '+8.2%' },
    { label: 'API Request Latency', value: '28ms', change: '-8ms' },
  ]);
}
