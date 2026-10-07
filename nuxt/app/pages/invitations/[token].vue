<script setup lang="ts">
definePageMeta({ middleware: 'auth' })


const route = useRoute()
const token = route.params.token as string

const { apiFetch } = useApi()
const status = ref<'pending' | 'success' | 'error'>('pending')
const message = ref('')

const acceptInvitation = async () => {
  try {

    await apiFetch(`/api/orgs/invitations/${token}/accept`, {method: 'POST'})
    status.value = 'success'
    message.value = 'Invitation accepted! Redirecting...'
    setTimeout(() => navigateTo('/orgs'), 1500)
  } catch (e: any) {
    status.value = 'error'
    message.value = e?.data?.error || 'Failed to accept invitation.'
  }


}

onMounted(acceptInvitation)
</script>

<template>
  <div class="min-h-screen flex items-center justify-center">
    <div class="bg-white p-8 rounded shadow max-w-sm text-center">


      <p v-if="status === 'pending'" class="text-gray-500">Accepting invitation...</p>
      <p v-else-if="status === 'success'" class="text-green-700">{{ message }}</p>
      <p v-else class="text-red-600">{{ message }}</p>
    </div>
  </div>
</template>