// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',


  	vite: {
  		define: {
  			'import.meta.url': JSON.stringify('http://localhost')
  		}
  	},
  devtools: { enabled: true }
})
