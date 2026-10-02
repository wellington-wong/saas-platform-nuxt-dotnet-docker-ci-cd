<script setup lang="ts">

const authStore = useAuthStore()


const { apiFetch } = useApi()

const fetchUser = async () => {
  if (!authStore.token || authStore.user) return
  try {
    authStore.user = await apiFetch<{ id: string; email: string; fullName: string }>('/api/users/me')
  } catch {
    authStore.logout()
    await navigateTo('/login')
  }
}
const handleLogout = () => {
  authStore.logout()
  navigateTo('/login')
}

onMounted(fetchUser)
</script>

<template>
  <div class="min-h-screen bg-gray-50">


    <nav v-if="authStore.isAuthenticated" class="bg-white border-b px-6 py-3 flex justify-between items-center">
      <NuxtLink to="/orgs" class="font-semibold">Sazzle</NuxtLink>
      <div class="flex items-center gap-4 text-sm">
        <span class="text-gray-500">{{ authStore.user?.email }}</span>
        <button @click="handleLogout" class="text-red-600 hover:underline">Log out</button>
      </div>
    </nav>
    <slot />

  </div>
</template>