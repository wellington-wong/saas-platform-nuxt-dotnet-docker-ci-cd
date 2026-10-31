<script setup lang="ts">
import OrganizationMembersPage from '~/components/OrganizationMembersPage.vue'
definePageMeta({ middleware: 'auth' })
const workspace = useWorkspace()
const orgId = computed(() => workspace.selectedOrgId.value || '')
onMounted(workspace.restoreOrganization)
</script>
<template>
  <OrganizationMembersPage v-if="orgId" :key="orgId" :org-id="orgId" />
  <div v-else class="select-org"><h1>Select an organization</h1><p>Choose an organization to manage its members.</p><NuxtLink to="/orgs">View organizations</NuxtLink></div>
</template>
<style scoped>.select-org { max-width: 680px; margin: 48px auto; padding: 28px; background: white; border: 1px solid #e9eaed; border-radius: 10px; }.select-org h1 { margin: 0 0 8px; font-size: 20px; }.select-org p, .select-org a { color: #777b85; font-size: 13px; }.select-org a { color: #5751bf; text-decoration: none; }</style>
