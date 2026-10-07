export default defineNuxtPlugin(async () => {
    const authStore = useAuthStore()
    authStore.loadFromStorage()


    const { apiFetch } = useApi()
    if (!authStore.token || authStore.user) return
    try {
        authStore.user = await apiFetch<{ id: string; email: string; fullName: string }>('/api/users/me')
    } catch {
        authStore.logout()
    }
})