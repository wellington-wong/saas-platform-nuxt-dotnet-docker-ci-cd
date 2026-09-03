<script setup lang="ts">

definePageMeta({ middleware: 'auth' })

interface Member {
  userId: string
  email: string
  fullName: string
  roleName: string
  joinedAt: string
}

interface Org {

  id: string
  name: string
  slug: string
}

const route = useRoute()
const orgId = route.params.id as string

const { apiFetch } = useApi()

const authStore = useAuthStore()
const org = ref<Org | null>(null)
const members = ref<Member[]>([])
const loading = ref(true)
const error = ref('')

const inviteEmail = ref('')
const inviteRoleId = ref('')
const inviting = ref(false)

const inviteResult = ref('')
const canInvite = ref(true) // becomes false if a 403 is hit

const roles = ref<{ id: string; name: string }[]>([])

const fetchMembers = async () => {
  try {
    members.value = await apiFetch<Member[]>(`/api/orgs/${orgId}/members`)



  } catch (e: any) {
    error.value = e?.data?.error || 'Failed to load members'
  }
}

const fetchOrgs = async () => {
  const orgs = await apiFetch<Org[]>('/api/orgs/me')
  org.value = orgs.find(o => o.id === orgId) || null }


const sendInvite = async () => {
  inviting.value = true
  inviteResult.value = ''

  error.value = ''
  try {
    const result = await apiFetch<{ token: string; email: string }>(`/api/orgs/${orgId}/invitations`, {
      method: 'POST',

      body: {email: inviteEmail.value, roleId: inviteRoleId.value}
    })
    inviteResult.value = `Invitation sent. Share this link: ${window.location.origin}/invitations/${result.token}`
    inviteEmail.value = ''
  } catch (e: any) {
    if (e?.reponse?.status === 403) {
      canInvite.value = false
      error.value = "You don't have permission to invite members."
    } else {
      error.value = e?.data?.error || 'Failed to send invitation.'
    }

  } finally {
    inviting.value = false

  }
}

const removeMember = async (userId: string) => {
  if (!confirm('Remove this member from the organization?')) return

  try {

    await apiFetch(`/api/orgs/${orgId}/members/${userId}`, {method: 'DELETE'})
    await fetchMembers()
  } catch (e: any) {
    error.value = e?.data?.error || 'Failed to remove member.'
  }
}

onMounted(async () => {
  loading.value = true

  await Promise.all([fetchOrgs(), fetchMembers()])
  loading.value = false
})
</script>

<template>
  <div class="max-w-2xl mx-auto p-8">




    <NuxtLink to="/orgs" class="text-blue-600 text-sm mb-4 inline-block">&larr; Back to organizations</NuxtLink>

    <div v-if="loading" class="text-gray-500">Loading...</div>

    <template v-else>
      <h1 class="text-2xl font-semibold mb-1">{{ org?.name }}</h1>
      <p class="text-gray-400 text-sm mb-6">/{{ org?.slug }}</p>
      <div class="bg-white rounded shadow mb-6">


        <h2 class="font-medium p-4 border-b">Members</h2>
        <ul>
          <li v-for="m in members" :key="m.userId"
              class="flex justify-between items-center p-4 border-b last:border-0">
            <div>
              <p class="font-medium">{{ m.fullName }}</p>
              <p class="text-gray-400 text-sm">{{ m.email }} &middot; {{ m.roleName }}</p>
            </div>

            <button v-if="m.userId !== authStore.user?.id"
                    @click="removeMember(m.userId)"
                    class="text-red-600 text-sm hover:underline">
              Remove
            </button>
          </li>
        </ul>
      </div>


      <div v-if="canInvite" class="bg-white p-6 rounded shadow">
        <h2 class="font-medium mb-4">Invite a member</h2>
        <form @submit.prevent="sendInvite" class="space-y-3">
          <input v-model="inviteEmail" type="email" placeholder="Email address" required
                 class="w-full border rounded px-3 py-2" />
          <input v-model="inviteRoleId" placeholder="Role ID (temporary - see note below)" required
                 class="w-full border rounded px-3 py-2" />
          <button type="submit" :disabled="inviting"
                  class="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 disabled:opacity-50">

            {{ inviting ? 'Sending...' : 'Send Invitation' }}
          </button>
        </form>

        <p v-if="inviteResult" class="text-green-700 text-sm mt-3 break-all">{{ inviteResult }}</p>
        <p v-if="error" class="text-red-600 text-sm mt-3">{{ error }}</p>
      </div>
    </template>
  </div>

</template>