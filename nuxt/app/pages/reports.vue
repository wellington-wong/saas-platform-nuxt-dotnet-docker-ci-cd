<script setup lang="ts">
import WorkspaceModulePage from '~/components/WorkspaceModulePage.vue'
definePageMeta({ middleware: 'auth' })
</script>
<template><WorkspaceModulePage section="reports" /></template>
