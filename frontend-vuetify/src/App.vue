<script setup>
import { ref, computed } from 'vue'
import Form from '@/components/Form.vue'
import Check from '@/components/Check.vue'

const routes = {
  '/': Form,
  '/check': Check
}

const currentPath = ref(window.location.hash)

window.addEventListener('hashchange', () => {
  currentPath.value = window.location.hash
})

const currentView = computed(() => {
  return routes[currentPath.value.slice(1) || '/'] || NotFound
})
</script>

<template>
  <v-head>
    
  <a href="#/">Employee Tracking Form</a>
  <a href="#/check">Report Page</a>
  </v-head>
<v-app>
  <v-main>
  <component :is="currentView" />
</v-main>
</v-app>
</template>
<style>
a {
  margin-right: 10px;
  font-size: 20px;
  margin-bottom: 150px;
  text-underline-offset: 1px;
  color: #b3acac;
  font-weight: bold;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;

  
}
a:hover {
  color: #ea8b8b;
  text-decoration: none;
}
</style>