<script setup lang="ts">
definePageMeta({ middleware: 'auth' })

type Org = { id: string; name: string; slug: string }
type Team = { id: string; organizationId: string; name: string }
const route = useRoute()
const workspace = useWorkspace()
const { apiFetch } = useApi()
const orgId = computed(() => String(route.params.id || workspace.selectedOrgId.value || ''))
const org = ref<Org | null>(null)
const teams = ref<Team[]>([])
const name = ref('')
const loading = ref(false)
const error = ref('')
const notice = ref('')

onMounted(async () => {
  workspace.restoreOrganization()
  await loadOrganization()
})
watch(orgId, loadOrganization)
async function loadOrganization() {
  if (!orgId.value) { org.value = null; return }
  try {
    const orgs = await apiFetch<Org[]>('/api/orgs/me')
    org.value = orgs.find(item => item.id === orgId.value) || null
  } catch (e: any) {
    error.value = e?.data?.error || 'Could not load this organization.'
  }
}

async function createTeam() {
  const teamName = name.value.trim()
  if (!teamName || loading.value || !orgId.value) return
  loading.value = true
  error.value = ''
  notice.value = ''
  try {
    const team = await apiFetch<Team>(`/api/orgs/${orgId.value}/teams`, { method: 'POST', body: { name: teamName } })
    teams.value.unshift(team)
    name.value = ''
    notice.value = `${team.name} was added to ${org.value?.name || 'this organization'}.`
  } catch (e: any) {
    error.value = e?.data?.error || 'Could not create the team. Check that you have permission to manage teams.'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="teams-page">
    <p class="eyebrow">{{ org?.name || 'Workspace' }} <span>/</span> People</p>
    <div class="page-heading"><div><h1>Teams</h1><p>Organize people into teams within this organization.</p></div><span class="team-count">{{ teams.length }} created this visit</span></div>

    <div v-if="!orgId" class="no-workspace"><h2>Select an organization</h2><p>Choose an organization before creating a team.</p><NuxtLink to="/orgs">View organizations</NuxtLink></div>
    <template v-else>
    <section class="create-card">
      <div class="card-heading"><span class="card-icon">✦</span><div><h2>Create a team</h2><p>Teams belong to this organization. Members can join multiple teams.</p></div></div>
      <form @submit.prevent="createTeam"><label for="team-name">Team name</label><div class="form-row"><input id="team-name" v-model="name" maxlength="80" placeholder="e.g. Product Design" required /><button type="submit" :disabled="loading || !name.trim()">{{ loading ? 'Creating…' : 'Create team' }}</button></div></form>
      <p v-if="error" class="message error" role="alert">{{ error }}</p><p v-if="notice" class="message success" role="status">{{ notice }}</p>
    </section>
    </template>

    <section class="team-section"><div class="section-heading"><h2>Teams</h2><span>{{ teams.length }}</span></div>
      <div v-if="teams.length" class="team-list"><article v-for="team in teams" :key="team.id" class="team-row"><span class="team-mark">{{ team.name.slice(0, 1).toUpperCase() }}</span><div><strong>{{ team.name }}</strong><small>Organization team</small></div><span class="team-members">Members can be added from team settings</span></article></div>
      <div v-else class="empty-state"><span class="empty-icon">♧</span><h3>No teams created in this visit</h3><p>Create a team above. Each team stays part of {{ org?.name || 'this organization' }} and can have its own members.</p></div>
    </section>
  </div>
</template>

<style scoped>
.teams-page { width: min(100%, 1080px); margin: 0 auto; padding: 36px 40px 64px; color: #25262b; }.eyebrow { margin: 0 0 10px; color: #858993; font-size: 12px; }.eyebrow span { margin: 0 6px; color: #c3c5ca; }.page-heading { display: flex; justify-content: space-between; align-items: center; gap: 16px; margin-bottom: 26px; }.page-heading h1 { margin: 0; font-size: 27px; letter-spacing: -.7px; font-weight: 650; }.page-heading p { margin: 7px 0 0; color: #777b85; font-size: 13px; }.team-count { padding: 7px 10px; color: #777b85; background: white; border: 1px solid #e8e9ed; border-radius: 20px; font-size: 11px; white-space: nowrap; }
.create-card, .team-section { background: white; border: 1px solid #e9eaed; border-radius: 10px; }.create-card { padding: 20px; }.card-heading { display: flex; align-items: center; gap: 12px; margin-bottom: 19px; }.card-icon, .team-mark, .empty-icon { display: grid; place-items: center; width: 36px; height: 36px; border-radius: 10px; background: #f0efff; color: #625bcc; font-size: 16px; }.card-heading h2, .section-heading h2 { margin: 0; font-size: 14px; font-weight: 620; }.card-heading p { margin: 4px 0 0; color: #858993; font-size: 11px; }.create-card label { display: block; margin-bottom: 7px; color: #5f626b; font-size: 11px; font-weight: 600; }.form-row { display: flex; gap: 9px; }.form-row input { min-width: 0; flex: 1; height: 38px; padding: 0 11px; border: 1px solid #e1e3e7; border-radius: 7px; outline: none; font: inherit; font-size: 12px; }.form-row input:focus { border-color: #8984df; box-shadow: 0 0 0 3px #7770db1c; }.form-row button { padding: 0 14px; border: 0; border-radius: 7px; color: white; background: #5b55c8; font-size: 11px; font-weight: 600; cursor: pointer; }.form-row button:disabled { opacity: .5; cursor: not-allowed; }.message { margin: 12px 0 0; font-size: 12px; }.error { color: #b04444; }.success { color: #27804d; }
.team-section { margin-top: 18px; padding: 17px 20px; }.section-heading { display: flex; align-items: center; gap: 8px; margin-bottom: 12px; }.section-heading span { color: #858993; font-size: 11px; }.empty-state { display: flex; flex-direction: column; align-items: center; text-align: center; padding: 32px 18px 38px; border-top: 1px solid #eff0f2; }.empty-state h3 { margin: 12px 0 5px; font-size: 13px; font-weight: 620; }.empty-state p { max-width: 390px; margin: 0; color: #858993; font-size: 11px; line-height: 1.6; }.team-list { border-top: 1px solid #eff0f2; }.team-row { display: flex; align-items: center; gap: 11px; padding: 12px 2px; border-bottom: 1px solid #eff0f2; }.team-mark { width: 32px; height: 32px; flex: 0 0 32px; }.team-row div { display: flex; flex: 1; flex-direction: column; gap: 3px; }.team-row strong { font-size: 12px; }.team-row small, .team-members { color: #858993; font-size: 10px; }.team-members { margin-left: auto; }
.no-workspace { padding: 30px; background: white; border: 1px solid #e9eaed; border-radius: 10px; }.no-workspace h2 { margin: 0 0 7px; font-size: 15px; }.no-workspace p { color: #777b85; font-size: 12px; }.no-workspace a { color: #5751bf; font-size: 12px; text-decoration: none; }
@media(max-width: 700px) { .teams-page { padding: 26px 18px 48px; }.page-heading { align-items: flex-start; flex-direction: column; }.team-count { align-self: flex-start; }.team-members { display: none; }.form-row { align-items: stretch; flex-direction: column; }.form-row button { min-height: 38px; } }
</style>
