import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-team',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="p-8 space-y-6 max-w-7xl w-full mx-auto">
      <div class="flex justify-between items-center">
        <div>
          <h2 class="text-xl font-bold text-slate-900">Team Members</h2>
          <p class="text-sm text-slate-500 mt-1">Manage permissions and workspace roles.</p>
        </div>
        <button
          class="px-4 py-2 bg-indigo-600 text-white text-xs font-semibold rounded-lg hover:bg-indigo-700 transition-colors"
        >
          Invite Member
        </button>
      </div>

      <div class="rounded-xl border border-slate-200 bg-white shadow-xs divide-y divide-slate-100">
        @for (member of members(); track member.email) {
          <div class="p-4 flex items-center justify-between">
            <div class="flex items-center space-x-3">
              <div
                class="h-9 w-9 rounded-full bg-slate-200 flex items-center justify-center font-bold text-xs text-slate-700"
              >
                {{ member.initials }}
              </div>
              <div>
                <div class="text-sm font-medium text-slate-900">{{ member.name }}</div>
                <div class="text-xs text-slate-400">{{ member.email }}</div>
              </div>
            </div>
            <span class="text-xs font-semibold bg-slate-100 text-slate-700 px-2.5 py-1 rounded-md">
              {{ member.role }}
            </span>
          </div>
        }
      </div>
    </div>
  `,
})
export class Team {
  members = signal([
    { name: 'Alex Rivera', email: 'alex&#64;requestrain.com', role: 'Owner', initials: 'AR' },
    {
      name: 'David Kim',
      email: 'david&#64;requestrain.com',
      role: 'Administrator',
      initials: 'DK',
    },
    {
      name: 'Jessica Taylor',
      email: 'jess&#64;requestrain.com',
      role: 'Developer',
      initials: 'JT',
    },
  ]);
}
