<script setup lang="ts">
import OrganizationMembersPage from '~/components/OrganizationMembersPage.vue'
definePageMeta({ middleware: 'auth' })
const route = useRoute()
const orgId = computed(() => String(route.params.id))
</script>
<template><OrganizationMembersPage :key="orgId" :org-id="orgId" /></template>
