<template lang="pug">
  form.buttons( @submit.prevent="handleSubmit" v-if="!currentPlatform.published")
    button.btn.btn-huge.btn-export.center.my-3(type="submit" :disabled="completed") Go live!
    p(v-if="currentError" class="error")
      strong {{currentError.message}}
    ul(v-if="currentError && currentError.errors")
      li(v-for="err in currentError.errors") {{err}}
</template>

<script lang="ts">
import { mapState, mapActions, mapGetters } from 'vuex'
export default {
  computed: {
    ...mapGetters('projects', ['currentPlatform', 'currentProject', 'currentError'])
  },
  data() {
    return {
      submitted: false,
      completed: false,
      error: '',
      result: {}
    }
  },
  methods: {
    ...mapActions('projects', ['goLive']),
    handleSubmit(e) {
      if (this.submitted) {
        return false
      }
      this.submitted = true
      this.error = ''
      this.goLive(this.currentProject.id)
        .then((response) => {
          this.completed = true
        })
        .catch((error) => {
          alert(error)
          this.completed = true
        })
    }
  }
}
</script>
