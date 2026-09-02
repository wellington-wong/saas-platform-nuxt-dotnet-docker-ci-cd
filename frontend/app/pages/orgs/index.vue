<script setup lang="ts">
definePageMeta({ middleware: 'auth' })


interface Org {
  id: string
  name: string
  slug: string
}

const { apiFetch } = useApi()
const orgs = ref<Org[]>([])
const loading = ref(true)

const error = ref('')
const newOrgName = ref('')
const newOrgSlug = ref('')
const creating = ref(false)

const fetchOrgs = async () => {
  loading.value = true
  try {
    orgs.value = await apiFetch<Org[]>('/api/orgs/me')

  } catch (e: any) {
    error.value = e?.data?.error || 'Failed to load organizations.'
  } finally {
    loading.value = false
  }
}

const createOrg = async () => {
  creating.value = true

  error.value = ''
  try {
    await apiFetch('/api/orgs', {
      method: 'POST',
      body: {name: newOrgName.value, slug: newOrgSlug.value}
    })
    newOrgName.value = ''
    newOrgSlug.value = ''


    await fetchOrgs()
  } catch (e: any) {
    error.value = e?.data?.error || 'Failed to create organization.'
  } finally {
    creating.value = false
  }
}

onMounted(fetchOrgs)

</script>

<template>
  <div class="max-w-2xl mx-auto p-8">
    <h1 class="text-2xl font-semibold mb-6">Your Organizations</h1>

    <div v-if="loading" class="text-gray-500">Loading...</div>

    <ul v-else class="space-y-2 mb-8">
      <li v-for="org in orgs" :key="org.id">
        <NuxtLink :to="`/orgs/${org.id}`" class="block p-4 bg-white rounded shadow hover:bg-gray-50">
          <span class="font-medium">{{ org.name }}</span>
          <span class="text-gray-400 text-sm ml-2">/{{ org.slug }}</span>
        </NuxtLink>
      </li>
      <li v-if="orgs.length === 0" class="text-gray-500">
        You don't belong to any organizations yet.
      </li>
    </ul>

    <div class="bg-white p-6 rounded shadow">
      <h2 class="font-medium mb-4">Create a new organization</h2>
      <form @submit.prevent="createOrg" class="space-y-3">
        <input v-model="newOrgName" placeholder="Organization name" required
               class="w-full border rounded px-3 py-2" />
        <input v-model="newOrgSlug" placeholder="url-slug" required
               class="w-full border rounded px-3 py-2" />
        <p v-if="error" class="text-red-600 text-sm">{{ error }}</p>
        <button type="submit" :disabled="creating"

                class="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 disabled:opacity-50">
          {{ creating ? 'Creating...' : 'Creating Organization' }}
        </button>
      </form>
    </div>
  </div>
</template>
