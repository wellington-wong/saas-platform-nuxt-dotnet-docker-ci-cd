export function useWorkspace() {
  const selectedOrgId = useState<string | null>('selected-workspace-org-id', () => null)

  function selectOrganization(id: string | null) {
    selectedOrgId.value = id
    if (import.meta.client) {
      if (id) localStorage.setItem('sazzle_selected_org_id', id)
      else localStorage.removeItem('sazzle_selected_org_id')
    }
  }

  function restoreOrganization() {
    if (import.meta.client && !selectedOrgId.value) {
      selectedOrgId.value = localStorage.getItem('sazzle_selected_org_id')
    }
  }

  return { selectedOrgId, selectOrganization, restoreOrganization }
}
