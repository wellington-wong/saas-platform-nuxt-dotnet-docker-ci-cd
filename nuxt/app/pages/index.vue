<script setup lang="ts">
const authStore = useAuthStore()


onMounted(async () => {
  authStore.loadFromStorage()
  if (authStore.isAuthenticated) {
    navigateTo('/orgs')
  } else {
    navigateTo('/login')
  }
})


</script>

<template>
  <div class="min-h-screen flex items-center justify-center">
    <p class="text-gray-500">Loading...</p>
  </div>
</template>