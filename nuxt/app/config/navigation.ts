export type WorkspaceRole = 'Super Admin' | 'Organization Owner' | 'Organization Admin' | 'Team Admin' | 'Team Member' | 'Viewer'
export type NavigationItem = { label: string; to: string; icon: string; permission?: keyof typeof roleAccess; roles?: WorkspaceRole[] }

export function normalizeWorkspaceRole(name?: string): WorkspaceRole {
  const normalized = name?.trim().toLowerCase()
  if (normalized === 'owner' || normalized === 'organization owner') return 'Organization Owner'
  if (normalized === 'admin' || normalized === 'organization admin') return 'Organization Admin'
  if (normalized === 'team admin') return 'Team Admin'
  if (normalized === 'super admin' || normalized === 'superadmin') return 'Super Admin'
  if (normalized === 'viewer') return 'Viewer'
  return 'Team Member'
}

const admins: WorkspaceRole[] = ['Super Admin', 'Organization Owner', 'Organization Admin']
const roleAccess = {
  manageOrganization: admins,
  manageRoles: ['Super Admin', 'Organization Owner', 'Organization Admin'] as WorkspaceRole[],
  manageBilling: ['Super Admin', 'Organization Owner', 'Organization Admin'] as WorkspaceRole[],
  managePeople: ['Super Admin', 'Organization Owner', 'Organization Admin', 'Team Admin'] as WorkspaceRole[],
}

export const navigationGroups = [
  { label: 'Workspace', items: [
    { label: 'Dashboard', to: 'dashboard', icon: 'grid' },
    { label: 'Activity', to: 'activity', icon: 'inbox' },
    { label: 'Projects', to: 'projects', icon: 'layers' },
    { label: 'Tasks', to: 'tasks', icon: 'check' },
    { label: 'Documents', to: 'documents', icon: 'file' },
    { label: 'Reports & Analytics', to: 'reports', icon: 'chart' },
  ] },
  { label: 'People', items: [
    { label: 'Members', to: 'members', icon: 'users', permission: 'managePeople' },
    { label: 'Teams', to: 'teams', icon: 'team', permission: 'managePeople' },
  ] },
  { label: 'Administration', items: [
    { label: 'Organization', to: 'organization', icon: 'building', permission: 'manageOrganization' },
    { label: 'Roles & Permissions', to: 'roles', icon: 'shield', permission: 'manageRoles' },
    { label: 'Billing & Subscription', to: 'billing', icon: 'credit', permission: 'manageBilling' },
  ] },
] satisfies { label: string; items: NavigationItem[] }[]

export function canSeeNavigation(role: WorkspaceRole, item: NavigationItem) {
  if (!item.permission) return true

  return roleAccess[item.permission].includes(role)
}
