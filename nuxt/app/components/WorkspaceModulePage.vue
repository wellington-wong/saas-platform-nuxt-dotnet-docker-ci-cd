<script setup lang="ts">
type Org = { id: string; name: string; slug: string }
type Role = { id: string; name: string }
const props = defineProps<{ section: string }>()
const page = useRoute()
const workspace = useWorkspace()
const { apiFetch } = useApi()
const orgId = computed(() => String(page.params.id || workspace.selectedOrgId.value || ''))
const section = computed(() => props.section)
const org = ref<Org | null>(null)
const roles = ref<Role[]>([])
const loadError = ref('')
const pageInfo: Record<string, { title: string; eyebrow: string; description: string; emptyTitle: string; emptyText: string }> = {
  dashboard: { title: 'Dashboard', eyebrow: 'Overview', description: 'A clear view of what is happening across your organization.', emptyTitle: 'Your workspace is ready', emptyText: 'Projects, tasks, and activity will appear here as your team starts working.' },
  activity: { title: 'Activity', eyebrow: 'Workspace', description: 'Recent updates from your organization in one place.', emptyTitle: 'No activity yet', emptyText: 'Updates will show up here when people make changes in this workspace.' },
  projects: { title: 'Projects', eyebrow: 'Workspace', description: 'Plan and track work across your organization.', emptyTitle: 'No projects yet', emptyText: 'Projects will give your teams a shared place to organize work.' },
  tasks: { title: 'Tasks', eyebrow: 'Workspace', description: 'Keep track of work that needs to get done.', emptyTitle: 'No tasks yet', emptyText: 'Tasks assigned to you and your teams will appear here.' },
  documents: { title: 'Documents', eyebrow: 'Workspace', description: 'Find the shared documents your team needs.', emptyTitle: 'No documents yet', emptyText: 'Shared workspace documents will be collected here.' },
  reports: { title: 'Reports & Analytics', eyebrow: 'Insights', description: 'Understand how work is progressing across your workspace.', emptyTitle: 'Reports are getting ready', emptyText: 'Analytics will appear as your workspace builds activity.' },
  organization: { title: 'Organization', eyebrow: 'Administration', description: 'Organization details and workspace identity.', emptyTitle: 'Organization details', emptyText: 'These details identify your organization across Sazzle.' },
  roles: { title: 'Roles & Permissions', eyebrow: 'Administration', description: 'Review the roles available to members of this organization.', emptyTitle: 'No roles found', emptyText: 'Roles could not be loaded for this organization.' },
  billing: { title: 'Billing & Subscription', eyebrow: 'Administration', description: 'Manage your organization subscription and billing details.', emptyTitle: 'Billing is not connected yet', emptyText: 'Subscription and payment details will be available here.' },
  settings: { title: 'Settings', eyebrow: 'Workspace', description: 'Manage preferences for this organization.', emptyTitle: 'Workspace settings', emptyText: 'More organization preferences will be available here.' },
}
const info = computed(() => pageInfo[section.value])

watch([orgId, section], async ([id, key]) => {
  loadError.value = ''
  if (!id) { org.value = null; return }
  try {
    const orgs = await apiFetch<Org[]>('/api/orgs/me')
    org.value = orgs.find(item => item.id === id) || null
    if (key === 'roles') roles.value = await apiFetch<Role[]>('/api/roles')
  } catch (error: any) {
    loadError.value = error?.data?.error || 'Unable to load workspace information.'
  }
}, { immediate: true })
</script>

<template>
  <div v-if="info" class="module-page">
    <div v-if="!orgId" class="empty-state module-empty"><span class="empty-icon">⌂</span><h2>Select an organization</h2><p>Choose a workspace to see its {{ info.title.toLowerCase() }}.</p><NuxtLink to="/orgs">View organizations</NuxtLink></div>
    <template v-else>
    <div class="module-heading">
      <div><p class="module-eyebrow">{{ org?.name || 'Workspace' }} <span>/</span> {{ info.eyebrow }}</p><h1>{{ info.title }}</h1><p class="module-description">{{ info.description }}</p></div>
      <span class="workspace-badge"><span></span> Workspace</span>
    </div>

    <div v-if="section === 'dashboard'" class="dashboard-grid">
      <article class="metric-card"><span>Projects</span><strong>—</strong><small>Workspace activity will appear here</small></article>
      <article class="metric-card"><span>Open tasks</span><strong>—</strong><small>Tasks assigned across your teams</small></article>
      <article class="metric-card"><span>Members</span><strong>—</strong><small>People in this organization</small></article>
    </div>

    <section v-if="section === 'organization'" class="details-card">
      <div class="organization-mark">{{ org?.name?.slice(0, 1).toUpperCase() || 'O' }}</div>
      <div class="detail-line"><span>Organization name</span><strong>{{ org?.name || '—' }}</strong></div>
      <div class="detail-line"><span>Workspace URL</span><strong>{{ org ? `/${org.slug}` : '—' }}</strong></div>
      <div class="detail-line"><span>Organization ID</span><strong>{{ org?.id || '—' }}</strong></div>
    </section>

    <section v-else-if="section === 'roles'" class="details-card role-list">
      <p v-if="loadError" class="module-error">{{ loadError }}</p>
      <div v-else-if="roles.length" v-for="role in roles" :key="role.id" class="role-row"><span class="role-symbol">R</span><div><strong>{{ role.name }}</strong><small>Organization role</small></div></div>
      <div v-else class="empty-state"><span class="empty-icon">⌘</span><h2>{{ info.emptyTitle }}</h2><p>{{ info.emptyText }}</p></div>
    </section>

    <section v-else-if="section !== 'dashboard'" class="empty-state module-empty"><span class="empty-icon">{{ section === 'billing' ? '$' : section === 'reports' ? '↗' : '✦' }}</span><h2>{{ info.emptyTitle }}</h2><p>{{ info.emptyText }}</p></section>
    <p v-if="loadError && section !== 'roles'" class="module-error">{{ loadError }}</p>
    </template>
  </div>
  <div v-else class="module-page"><NuxtLink :to="`/orgs/${orgId}`" class="back-link">← Back to members</NuxtLink><h1>Page not found</h1><p>This workspace section does not exist.</p></div>
</template>

<style scoped>
.module-page { width: min(100%, 1120px); margin: 0 auto; padding: 36px 40px 64px; color: #25262b; }
.module-heading { display: flex; justify-content: space-between; align-items: flex-start; gap: 20px; margin-bottom: 29px; }
.module-eyebrow { margin: 0 0 10px; color: #858993; font-size: 12px; }.module-eyebrow span { margin: 0 6px; color: #c3c5ca; }
h1 { margin: 0; font-size: 27px; letter-spacing: -.7px; font-weight: 650; }.module-description { margin: 8px 0 0; color: #777b85; font-size: 13px; }
.workspace-badge { display: inline-flex; align-items: center; gap: 7px; border: 1px solid #e8e9ed; border-radius: 20px; padding: 7px 10px; color: #656974; font-size: 11px; background: white; }.workspace-badge span { width: 7px; height: 7px; background: #6eb58a; border-radius: 50%; }
.dashboard-grid { display: grid; grid-template-columns: repeat(3, minmax(0, 1fr)); gap: 14px; }.metric-card, .details-card, .empty-state { background: white; border: 1px solid #e9eaed; border-radius: 10px; }
.metric-card { display: flex; flex-direction: column; padding: 18px; min-height: 128px; }.metric-card span { color: #777b85; font-size: 12px; }.metric-card strong { margin: 10px 0 4px; font-size: 25px; font-weight: 620; }.metric-card small { color: #989ba3; font-size: 11px; }
.details-card { padding: 20px; max-width: 760px; }.organization-mark { width: 42px; height: 42px; display: grid; place-items: center; margin-bottom: 18px; border-radius: 10px; background: #efedff; color: #5b50c8; font-weight: 700; }.detail-line { display: flex; justify-content: space-between; gap: 20px; padding: 14px 0; border-top: 1px solid #eff0f2; font-size: 12px; }.detail-line span { color: #777b85; }.detail-line strong { font-weight: 560; overflow-wrap: anywhere; }
.empty-state { display: flex; flex-direction: column; align-items: center; text-align: center; padding: 58px 20px; }.module-empty { min-height: 290px; justify-content: center; }.empty-icon, .role-symbol { display: grid; place-items: center; width: 38px; height: 38px; border-radius: 11px; color: #625bcc; background: #f0efff; font-size: 18px; }.empty-state h2 { margin: 15px 0 5px; font-size: 15px; font-weight: 620; }.empty-state p { max-width: 400px; margin: 0; color: #858993; font-size: 12px; line-height: 1.6; }
.role-list { padding: 7px 18px; }.role-row { display: flex; align-items: center; gap: 12px; padding: 12px 2px; border-bottom: 1px solid #eff0f2; }.role-row:last-child { border: 0; }.role-symbol { width: 31px; height: 31px; font-size: 12px; }.role-row div { display: flex; flex-direction: column; gap: 3px; }.role-row strong { font-size: 12px; }.role-row small { color: #888c95; font-size: 11px; }.module-error { color: #b04444; font-size: 12px; }.back-link { color: #5b55c8; font-size: 12px; text-decoration: none; }
@media(max-width: 700px) { .module-page { padding: 26px 18px 48px; }.module-heading { display: block; }.workspace-badge { margin-top: 16px; }.dashboard-grid { grid-template-columns: 1fr; }.metric-card { min-height: 108px; } }
</style>
