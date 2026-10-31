<script setup lang="ts">
import AppSidebar from '~/components/sidebar/AppSidebar.vue'
import SidebarIcon from '~/components/sidebar/SidebarIcon.vue'

const auth = useAuthStore()
const route = useRoute()
const collapsed = ref(false)
const mobileOpen = ref(false)
const showAppShell = computed(() => route.path !== '/login' && route.path !== '/register')
watch(() => route.fullPath, () => { mobileOpen.value = false })
onMounted(() => { collapsed.value = localStorage.getItem('sazzle_sidebar_collapsed') === 'true' })
function toggleSidebar() {
  collapsed.value = !collapsed.value
  localStorage.setItem('sazzle_sidebar_collapsed', String(collapsed.value))
}
</script>

<template>
  <div class="app-frame" :class="{ 'with-sidebar': showAppShell, 'sidebar-collapsed': collapsed }">
    <template v-if="showAppShell">
      <AppSidebar :collapsed="collapsed" @toggle="toggleSidebar" />
      <div v-if="mobileOpen" class="mobile-scrim" @click="mobileOpen = false" />
      <div class="mobile-sidebar-host" :class="{ open: mobileOpen }"><AppSidebar mobile @close="mobileOpen = false" /></div>
      <main class="app-main">
        <header class="desktop-topbar">
          <NuxtLink to="/orgs" class="desktop-brand">Sazzle</NuxtLink>
          <div class="desktop-account"><span>{{ auth.user?.email || '' }}</span><button v-if="auth.isAuthenticated" type="button" @click="auth.logout(); navigateTo('/login')">Log out</button></div>
        </header>
        <header class="mobile-topbar"><button class="mobile-menu-button" aria-label="Open navigation" @click="mobileOpen = true"><SidebarIcon name="menu" /></button><NuxtLink to="/orgs" class="mobile-brand">sazzle</NuxtLink><span class="mobile-spacer" /></header>
        <slot />
      </main>
    </template>
    <template v-else><main class="auth-main"><slot /></main></template>
  </div>
</template>

<style>
:root { font-family: Inter, ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif; color: #202124; background: #f7f8fa; font-synthesis: none; }
* { box-sizing: border-box; }
body { margin: 0; }
.app-frame { min-height: 100vh; }
.app-frame.with-sidebar { --sidebar-width: 256px; display: flex; }
.app-frame.with-sidebar.sidebar-collapsed { --sidebar-width: 72px; }
.app-sidebar { width: var(--sidebar-width, 256px); height: 100vh; flex: 0 0 var(--sidebar-width, 256px); position: sticky; top: 0; display: flex; flex-direction: column; background: #fff; border-right: 1px solid #e8e9ec; padding: 15px 12px 12px; transition: width .22s ease, flex-basis .22s ease; z-index: 30; }
.sidebar-top { height: 38px; display: flex; align-items: center; justify-content: space-between; margin: 0 3px 18px; }
.brand { display: inline-flex; align-items: center; gap: 9px; color: #202124; text-decoration: none; min-width: 0; }
.brand-mark { width: 28px; height: 28px; display: grid; place-items: center; color: #fff; background: #5856d6; border-radius: 8px; font-size: 19px; line-height: 1; font-weight: 700; }
.brand-name { font-size: 17px; font-weight: 680; letter-spacing: -.5px; }
.collapse-button, .mobile-menu-button { border: 0; display: grid; place-items: center; width: 30px; height: 30px; padding: 6px; color: #727782; background: transparent; border-radius: 7px; cursor: pointer; }
.collapse-button:hover, .mobile-menu-button:hover { background: #f1f2f5; color: #34363b; }
.collapse-button svg, .mobile-menu-button svg { width: 17px; height: 17px; }
.org-wrap, .profile-wrap { position: relative; }
.org-switcher, .profile-button { width: 100%; display: flex; align-items: center; gap: 10px; border: 1px solid #eceef0; background: #fff; padding: 8px; text-align: left; border-radius: 9px; cursor: pointer; color: #26272b; }
.org-switcher:hover, .profile-button:hover { background: #f9fafb; }
.org-avatar { width: 32px; height: 32px; flex: 0 0 32px; display: grid; place-items: center; border-radius: 8px; background: #edeaff; color: #5b50c8; font-size: 12px; font-weight: 700; }
.org-avatar.small { width: 24px; height: 24px; flex-basis: 24px; border-radius: 6px; }
.org-copy, .profile-copy { min-width: 0; display: flex; flex: 1; flex-direction: column; gap: 3px; }
.org-copy strong, .profile-copy strong { font-size: 12px; font-weight: 620; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
.org-copy small, .profile-copy small { font-size: 11px; color: #858993; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
.switch-chevron { width: 14px; height: 14px; color: #90939b; flex: 0 0 auto; }
.navigation-list { flex: 1; overflow-y: auto; padding-top: 12px; }
.nav-group { margin-bottom: 16px; }
.nav-group h2 { margin: 8px 9px 6px; color: #9699a2; font-size: 10px; line-height: 1.2; text-transform: uppercase; letter-spacing: .075em; font-weight: 650; }
.nav-item { position: relative; display: flex; align-items: center; gap: 11px; height: 35px; padding: 0 10px; border-radius: 7px; color: #626670; font-size: 12.5px; font-weight: 480; text-decoration: none; margin: 1px 0; transition: background .15s, color .15s; }
.nav-item svg { width: 17px; height: 17px; flex: 0 0 17px; }
.nav-item:hover { color: #27292e; background: #f5f6f8; }
.nav-item.active { color: #4d48bd; background: #efefff; font-weight: 620; }
.sidebar-bottom { border-top: 1px solid #eceef0; padding-top: 9px; }
.sidebar-bottom > .nav-item { margin-bottom: 1px; }
.user-avatar { width: 29px; height: 29px; flex: 0 0 29px; display: grid; place-items: center; border-radius: 50%; color: #8c4828; background: #f5dfd1; font-weight: 650; font-size: 11px; }
.profile-button { border: 0; margin-top: 7px; padding: 7px 6px; }
.profile-copy strong { font-size: 11.5px; }
.sidebar-popover { position: absolute; z-index: 60; left: 0; right: 0; background: #fff; border: 1px solid #e7e8eb; border-radius: 10px; box-shadow: 0 10px 30px #1e25301c; padding: 6px; }
.org-menu { top: calc(100% + 6px); }
.popover-label { margin: 5px 8px 7px; color: #979aa3; font-size: 10px; font-weight: 650; text-transform: uppercase; letter-spacing: .07em; }
.org-option { display: flex; align-items: center; gap: 9px; min-height: 34px; padding: 5px 7px; color: #44474f; border-radius: 6px; font-size: 12px; text-decoration: none; }
.org-option:hover, .user-menu a:hover { background: #f5f6f8; }
.selected-check { margin-left: auto; color: #5b55c8; }
.create-org { border-top: 1px solid #eceef0; border-radius: 0; margin-top: 4px; padding-top: 9px; color: #696d76; }
.create-org svg { width: 15px; height: 15px; }
.user-menu { bottom: calc(100% + 7px); }
.user-menu-heading { display: flex; flex-direction: column; gap: 3px; padding: 8px 9px 10px; border-bottom: 1px solid #eceef0; margin-bottom: 4px; }
.user-menu-heading strong { font-size: 12px; }
.user-menu-heading small { color: #858993; font-size: 11px; }
.user-menu a, .sign-out { display: flex; align-items: center; min-height: 32px; padding: 0 9px; border: 0; border-radius: 6px; color: #484b53; background: transparent; text-decoration: none; font-size: 12px; cursor: pointer; }
.sign-out { width: 100%; border-top: 1px solid #eceef0; border-radius: 0; margin-top: 4px; padding-top: 4px; color: #b04444; gap: 8px; }
.sign-out svg { width: 15px; height: 15px; }
.app-main { flex: 1; min-width: 0; min-height: 100vh; }
.desktop-topbar { height: 54px; display: flex; align-items: center; justify-content: space-between; padding: 0 24px; background: #fff; border-bottom: 1px solid #e8e9ec; }
.desktop-brand { color: #27282d; font-size: 14px; font-weight: 650; text-decoration: none; }
.desktop-account { display: flex; align-items: center; gap: 16px; color: #737782; font-size: 12px; }
.desktop-account button { border: 0; padding: 5px 8px; color: #a64a4a; background: transparent; font: inherit; cursor: pointer; }
.auth-main { min-height: 100vh; }
.mobile-topbar, .mobile-sidebar-host, .mobile-scrim { display: none; }
.is-collapsed .brand-name, .is-collapsed .org-copy, .is-collapsed .switch-chevron, .is-collapsed .org-switcher .switch-chevron, .is-collapsed .nav-group h2, .is-collapsed .nav-item span, .is-collapsed .profile-copy, .is-collapsed .profile-button .switch-chevron { display: none; }
.is-collapsed .sidebar-top { justify-content: center; margin-left: 0; margin-right: 0; }
.is-collapsed .collapse-button { position: absolute; right: -12px; top: 49px; width: 23px; height: 23px; border: 1px solid #e8e9ec; background: white; border-radius: 50%; }
.is-collapsed .org-switcher, .is-collapsed .profile-button { justify-content: center; padding: 7px 0; border-color: transparent; }
.is-collapsed .org-avatar { margin: 0; }
.is-collapsed .nav-group { margin-bottom: 12px; }
.is-collapsed .nav-group .nav-item { justify-content: center; padding: 0; }
.is-collapsed .nav-item { position: relative; }
.is-collapsed .nav-item:hover::after { content: attr(title); position: absolute; left: calc(100% + 10px); top: 50%; transform: translateY(-50%); z-index: 80; white-space: nowrap; padding: 7px 9px; border: 1px solid #e6e7eb; border-radius: 6px; color: #363840; background: #fff; box-shadow: 0 5px 18px #1e25301c; font-size: 11px; }
.is-collapsed .sidebar-bottom { padding-top: 8px; }
.is-collapsed .sidebar-popover { left: calc(100% + 8px); right: auto; top: 0; width: 220px; bottom: auto; }
.is-collapsed .user-menu { top: auto; bottom: 0; }
@media (max-width: 760px) {
  .app-frame.with-sidebar, .app-frame.with-sidebar.sidebar-collapsed { display: block; }
  .app-frame.with-sidebar > .app-sidebar { display: none; }
  .mobile-topbar { height: 54px; display: flex; align-items: center; gap: 12px; position: sticky; top: 0; z-index: 20; background: #fff; border-bottom: 1px solid #e8e9ec; padding: 0 15px; }
  .desktop-topbar { display: none; }
  .mobile-brand { color: #27282d; text-decoration: none; font-size: 15px; font-weight: 680; }
  .mobile-spacer { flex: 1; }
  .mobile-sidebar-host { display: block; position: fixed; z-index: 50; inset: 0 auto 0 0; width: 280px; transform: translateX(-105%); transition: transform .22s ease; }
  .mobile-sidebar-host.open { transform: translateX(0); }
  .mobile-sidebar-host .app-sidebar { width: 280px; height: 100%; flex-basis: 280px; box-shadow: 12px 0 30px #171b251a; }
  .mobile-scrim { display: block; position: fixed; inset: 0; z-index: 40; background: #14182166; }
}
</style>
