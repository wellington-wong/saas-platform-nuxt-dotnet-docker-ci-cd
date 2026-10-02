export default defineNuxtRouteMiddleware((to) => {
    const authStore = useAuthStore()



    if (import.meta.server) return

    if (!authStore.isAuthenticated && to.path !== '/login' && to.path !== '/register') {
        return navigateTo('/login')
    }
})