export default defineNuxtRouteMiddleware((to) => {
    const authStore = useAuthStore()



    if (import.meta.server) return

    if (import.meta.client && !authStore.token) {
        authStore.loadFromStorage()
    }

    if (!authStore.isAuthenticated && to.path !== '/login' && to.path !== '/register') {
        return navigateTo('/login')
    }
})