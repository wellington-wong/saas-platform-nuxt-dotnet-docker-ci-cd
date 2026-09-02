export const useApi = () => {
    const config = useRuntimeConfig()
    const authStore = useAuthStore()

    console.log(config.public.apiBase)
    const apiFetch = $fetch.create({
        baseURL: config.public.apiBase,
        onRequest({options}) {
            if (authStore.token) {
                options.headers.set('Authorization', `Bearer ${authStore.token}`)
            }
        },
        onResponseError({response}) {

            if (response.status === 401) {
                authStore.logout()
                navigateTo('/login')
            }
        }
    });

    return {apiFetch}
}