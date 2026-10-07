import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="p-8 space-y-6 max-w-7xl w-full mx-auto">
      <div>
        <h2 class="text-xl font-bold text-slate-900">Workspace Settings</h2>
        <p class="text-sm text-slate-500 mt-1">
          Update global configuration parameters for your app.
        </p>
      </div>

      <div class="rounded-xl border border-slate-200 bg-white p-6 shadow-xs space-y-4">
        <h3 class="text-sm font-semibold text-slate-800">General Information</h3>
        <div class="space-y-3 max-w-xl">
          <div class="space-y-1">
            <label class="text-xs font-medium text-slate-700">Workspace Name</label>
            <input
              type="text"
              value="RequestRain Production"
              class="w-full px-3 py-2 text-sm rounded-lg border border-slate-200 focus:outline-hidden focus:ring-2 focus:ring-indigo-500/20 focus:border-indigo-600"
            />
          </div>
          <div class="space-y-1">
            <label class="text-xs font-medium text-slate-700">Edge Region Preference</label>
            <select
              class="w-full px-3 py-2 text-sm rounded-lg border border-slate-200 focus:outline-hidden focus:ring-2 focus:ring-indigo-500/20 focus:border-indigo-600"
            >
              <option>Automatic (Global Edge)</option>
              <option>North America (US)</option>
              <option>Europe (EU)</option>
            </select>
          </div>
          <button
            class="px-4 py-2 bg-slate-900 text-white text-xs font-semibold rounded-lg hover:bg-slate-800 transition-colors"
          >
            Save Changes
          </button>
        </div>
      </div>
    </div>
  `,
})
export class Settings {}
