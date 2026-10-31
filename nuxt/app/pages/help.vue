<script setup lang="ts">
definePageMeta({ middleware: 'auth' })
</script>

<template>
  <div class="help-page">
    <p class="eyebrow">Sazzle / Support</p><h1>Help &amp; Support</h1><p class="intro">Find a hand with your workspace or get in touch with our support team.</p>
    <div class="help-grid">
      <a class="help-card" href="mailto:support@sazzle.app"><span class="help-icon">✉</span><h2>Contact support</h2><p>Send us a message and include the organization you need help with.</p><span class="card-link">support@sazzle.app ↗</span></a>
      <NuxtLink class="help-card" to="/orgs"><span class="help-icon">⌂</span><h2>Organizations</h2><p>Switch between your workspaces or manage the organizations you belong to.</p><span class="card-link">View organizations →</span></NuxtLink>
    </div>
  </div>
</template>

<style scoped>
.help-page { width: min(100%, 1080px); margin: 0 auto; padding: 36px 40px 64px; color: #25262b; }.eyebrow { margin: 0 0 10px; color: #858993; font-size: 12px; }.help-page h1 { margin: 0; font-size: 27px; letter-spacing: -.7px; }.intro { margin: 8px 0 26px; color: #777b85; font-size: 13px; }.help-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 14px; }.help-card { display: block; padding: 20px; color: inherit; text-decoration: none; background: white; border: 1px solid #e9eaed; border-radius: 10px; transition: border-color .15s, transform .15s; }.help-card:hover { border-color: #c9c6ee; transform: translateY(-1px); }.help-icon { display: grid; place-items: center; width: 36px; height: 36px; border-radius: 10px; background: #f0efff; color: #625bcc; }.help-card h2 { margin: 16px 0 6px; font-size: 14px; }.help-card p { margin: 0 0 17px; max-width: 340px; color: #858993; font-size: 11px; line-height: 1.6; }.card-link { color: #5751bf; font-size: 11px; font-weight: 600; }
@media(max-width: 700px) { .help-page { padding: 26px 18px 48px; }.help-grid { grid-template-columns: 1fr; } }
</style>
