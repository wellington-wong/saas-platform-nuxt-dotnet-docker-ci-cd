<script setup lang="ts">

const email = ref('')

const password = ref('')
const error = ref('')
const loading = ref(false)
const { apiFetch } = useApi()
const authStore = useAuthStore()

const handleLogin = async () => {
  error.value = ''
  loading.value = true


  try {
    const response = await apiFetch<{ token: string }>('/api/auth/login', {
      method: 'POST',
      body: {email: email.value, password: password.value}
    })

    const me = await apiFetch<{ id: string; email: string; fullName: string }>('api/users/me', {
      headers: {Authorization: `Bearer ${response.token}`}

    })

    authStore.setSession(response.token, me)
    navigateTo('/orgs')

  } catch (e: any) {
    error.value = e?.data?.error || 'Login failed. Check your credentials.'
  } finally {
    loading.value = false


  }
}
</script>

<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-50">
    <form @submit.prevent="handleLogin" class="bg-white p-8 rounded-lg shadow-md w-full max-w-sm">



      <h1 class="text-2xl font-semibold mb-6">Log in to Sazzle</h1>
      <div class="mb-4">
        <label class="block text-sm font-medium mb-1">Email</label>
        <input v-model="email" type="email" required class="w-full border rounded px-3 py-2" />
      </div>

      <div class="mb-4">
        <label class="block text-sm font-medium mb-1">Password</label>

        <input v-model="password" type="password" required class="w-full border rounded px-3 py-2" />
      </div>

      <p v-if="error" class="text-red-600 text-sm mb-4">{{ error }}</p>

      <button type="submit" :disabled="loading" class="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 disabled:opacity-50">
        {{ loading ? 'Logging in...' : 'Log in' }}
      </button>




      <p class="text-sm text-center mt-4">
        No account?
        <NuxtLink to="/register" class="text-blue-600">Register</NuxtLink>
      </p>
    </form>
  </div>
</template>