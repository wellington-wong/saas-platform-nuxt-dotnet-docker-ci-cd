// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
    compatibilityDate: '2025-07-15',

    vite: {
        define: {
         // 'import.meta.url': JSON.stringify('http://localhost')
        }
    },

    devtools: { enabled: true },
    modules: ['@pinia/nuxt', '@nuxtjs/tailwindcss'],
    runtimeConfig: {

        public: {
            apiBase: process.env.NUXT_PUBLIC_API_BASE || 'http://localhost:5080'
        }
    },
    nitro: {
        preset: 'cloudflare_pages'
    }

})