<script setup lang="ts">
import SidebarIcon from '~/components/sidebar/SidebarIcon.vue'
import { canSeeNavigation, navigationGroups, type WorkspaceRole } from '~/config/navigation'

type Org = { id: string; name: string; slug: string }
withDefaults(defineProps<{ mobile?: boolean; collapsed?: boolean }>(), { mobile: false, collapsed: false })
const emit = defineEmits<{ close: []; toggle: [] }>()
const route = useRoute()
const auth = useAuthStore()
const { apiFetch } = useApi()
const orgs = ref<Org[]>([])
const role = ref<WorkspaceRole>('Team Member')
const openOrg = ref(false)
const openUser = ref(false)
const currentOrgId = computed(() => String(route.params.id || ''))
const currentOrg = computed(() => orgs.value.find(org => org.id === currentOrgId.value))
const orgInitials = computed(() => (currentOrg.value?.name || 'S').slice(0, 1).toUpperCase())
const userInitials = computed(() => (auth.user?.fullName || auth.user?.email || 'U').slice(0, 1).toUpperCase())

function destination(to: string) {
  if (to === 'members') return currentOrgId.value ? `/orgs/${currentOrgId.value}` : '/orgs'
  if (to === 'organization') return currentOrgId.value ? `/orgs/${currentOrgId.value}?section=organization` : '/orgs'
  return currentOrgId.value ? `/orgs/${currentOrgId.value}?section=${to}` : '/orgs'
}
function isActive(to: string) {
  if (to === 'members') return route.path === `/orgs/${currentOrgId.value}` && !route.query.section
  if (to === 'organization') return route.query.section === 'organization'
  return route.query.section === to
}
async function loadWorkspace() {
  role.value = 'Team Member'
  try {
    orgs.value = await apiFetch<Org[]>('/api/orgs/me')
    if (currentOrgId.value && auth.user?.id) {
      const members = await apiFetch<{ userId: string; roleName: string }[]>(`/api/orgs/${currentOrgId.value}/members`)
      const name = members.find(member => member.userId === auth.user?.id)?.roleName
      const roles: WorkspaceRole[] = ['Super Admin', 'Organization Owner', 'Organization Admin', 'Team Admin', 'Team Member', 'Viewer']
      if (name && roles.includes(name as WorkspaceRole)) role.value = name as WorkspaceRole
    }
  } catch { /* Keep the safe Team Member role while workspace data is unavailable. */ }
}
function logout() { auth.logout(); navigateTo('/login') }
onMounted(loadWorkspace)
watch(currentOrgId, loadWorkspace)
</script>

<template>
  <aside class="app-sidebar" :class="{ 'is-mobile': mobile, 'is-collapsed': collapsed }" aria-label="Main navigation">
    <div class="sidebar-top">
      <NuxtLink to="/orgs" class="brand" aria-label="Sazzle home" @click="emit('close')"><span class="brand-mark">s</span><span class="brand-name">sazzle</span></NuxtLink>
      <button v-if="!mobile" class="collapse-button" type="button" :title="collapsed ? 'Expand sidebar' : 'Collapse sidebar'" :aria-label="collapsed ? 'Expand sidebar' : 'Collapse sidebar'" @click="emit('toggle')"><SidebarIcon name="panel" /></button>
      <button v-else class="collapse-button" type="button" aria-label="Close navigation" @click="emit('close')"><SidebarIcon name="close" /></button>
    </div>

    <div class="org-wrap">
      <button class="org-switcher" type="button" title="Switch organization" :aria-expanded="openOrg" @click="openOrg = !openOrg; openUser = false">
        <span class="org-avatar">{{ orgInitials }}</span><span class="org-copy"><strong>{{ currentOrg?.name || 'Your organizations' }}</strong><small>{{ currentOrg ? `/${currentOrg.slug}` : 'Workspace' }}</small></span><SidebarIcon name="down" class="switch-chevron" />
      </button>
      <div v-if="openOrg" class="sidebar-popover org-menu">
        <p class="popover-label">Switch organization</p>
        <NuxtLink v-for="org in orgs" :key="org.id" :to="`/orgs/${org.id}`" class="org-option" @click="openOrg = false; emit('close')"><span class="org-avatar small">{{ org.name.slice(0, 1).toUpperCase() }}</span><span>{{ org.name }}</span><span v-if="org.id === currentOrgId" class="selected-check">✓</span></NuxtLink>
        <NuxtLink to="/orgs" class="org-option create-org" @click="openOrg = false; emit('close')"><SidebarIcon name="plus" /> Manage organizations</NuxtLink>
      </div>
    </div>

    <nav class="navigation-list">
      <section v-for="group in navigationGroups" :key="group.label" class="nav-group">
        <h2>{{ group.label }}</h2>
        <template v-for="item in group.items" :key="item.to">
          <NuxtLink v-if="canSeeNavigation(role, item)" :to="destination(item.to)" class="nav-item" :class="{ active: isActive(item.to) }" :title="item.label" :aria-current="isActive(item.to) ? 'page' : undefined" @click="emit('close')">
            <SidebarIcon :name="item.icon" /><span>{{ item.label }}</span>
          </NuxtLink>
        </template>
      </section>
    </nav>

    <div class="sidebar-bottom">
      <NuxtLink :to="currentOrgId ? `/orgs/${currentOrgId}?section=settings` : '/orgs'" class="nav-item" title="Settings" @click="emit('close')"><SidebarIcon name="settings" /><span>Settings</span></NuxtLink>
      <a href="mailto:support@sazzle.app" class="nav-item" title="Help & Support"><SidebarIcon name="help" /><span>Help &amp; Support</span></a>
      <div class="profile-wrap">
        <button class="profile-button" type="button" title="User profile" :aria-expanded="openUser" @click="openUser = !openUser; openOrg = false"><span class="user-avatar">{{ userInitials }}</span><span class="profile-copy"><strong>{{ auth.user?.fullName || auth.user?.email || 'Account' }}</strong><small>{{ role }}</small></span><SidebarIcon name="down" class="switch-chevron" /></button>
        <div v-if="openUser" class="sidebar-popover user-menu">
          <div class="user-menu-heading"><strong>{{ auth.user?.fullName || 'Your account' }}</strong><small>{{ auth.user?.email }}</small></div>
          <NuxtLink to="/orgs" @click="openUser = false; emit('close')">My Profile</NuxtLink>
          <NuxtLink :to="currentOrgId ? `/orgs/${currentOrgId}?section=settings` : '/orgs'" @click="openUser = false; emit('close')">Account Settings</NuxtLink>
          <NuxtLink to="/orgs" @click="openUser = false; emit('close')">Switch Organization</NuxtLink>
          <button type="button" class="sign-out" @click="logout"><SidebarIcon name="logout" /> Sign Out</button>
        </div>
      </div>
    </div>
  </aside>
</template>
