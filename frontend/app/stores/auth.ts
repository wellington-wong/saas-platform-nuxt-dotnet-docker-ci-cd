interface User {
    id: string
    email: string
    fullName: string
}

export const useAuthStore = defineStore('auth', {
    state: () => ({
        token: null as string | null,
        user: null as user | null
    }),

    actions: {

        setSession(token: string, user: User) {
            this.token = token
            this.user = user
            if (import.meta.client) {
                localStorage.setItem('sazzle_token', token)
            }
        },

        loadFromStorage() {

            if (import.meta.client) {
                const token = localStorage.getItem('sazzle_token')
                if (token) this.token = token
            }
        },

        logout() {
            this.token = null
            this.user = null

            if (import.meta.client) {
                localStorage.removeItem('sazzle_token')
            }
        }
    },

    getters: {
        isAuthenticated: (state) => !!state.token
    }

})