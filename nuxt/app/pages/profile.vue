<script setup lang="ts">
definePageMeta({ middleware: 'auth' })
const auth = useAuthStore()
const initials = computed(() => (auth.user?.fullName || auth.user?.email || 'U').slice(0, 1).toUpperCase())
</script>

<template>
  <div class="profile-page"><p class="eyebrow">Account</p><h1>My Profile</h1><p class="intro">Your personal account details.</p>
    <section class="profile-card"><span class="avatar">{{ initials }}</span><div class="profile-detail"><span>Name</span><strong>{{ auth.user?.fullName || '—' }}</strong></div><div class="profile-detail"><span>Email</span><strong>{{ auth.user?.email || '—' }}</strong></div></section>
  </div>
</template>

<style scoped>
.profile-page { width: min(100%, 1080px); margin: 0 auto; padding: 36px 40px 64px; color: #25262b; }.eyebrow { margin: 0 0 10px; color: #858993; font-size: 12px; }.profile-page h1 { margin: 0; font-size: 27px; letter-spacing: -.7px; }.intro { margin: 8px 0 26px; color: #777b85; font-size: 13px; }.profile-card { max-width: 700px; padding: 20px; background: white; border: 1px solid #e9eaed; border-radius: 10px; }.avatar { display: grid; place-items: center; width: 44px; height: 44px; margin-bottom: 16px; border-radius: 50%; color: #8c4828; background: #f5dfd1; font-weight: 650; }.profile-detail { display: flex; justify-content: space-between; gap: 20px; padding: 13px 0; border-top: 1px solid #eff0f2; font-size: 12px; }.profile-detail span { color: #777b85; }.profile-detail strong { font-weight: 560; }
@media(max-width: 700px) { .profile-page { padding: 26px 18px 48px; } }
</style>
