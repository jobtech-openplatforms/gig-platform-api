<template lang="pug">
  .card.project-info.mt-4
    form.inline( @submit.prevent="handleSubmit")
      label(for="project-info-name")
        .label-text Name
        validation-provider(rules="required" v-slot="{ errors }")
          input(type="text" id="project-info-name" required v-model="name" placeholder="Project name" name="Name")
          .feedback
           .invalid-feedback {{ errors[0] }}
      button.btn.btn-project Create
    .error {{current.error}}
</template>

<script lang="ts">
import { mapState, mapActions } from 'vuex'

// TODO: Make this a class implementation - currently VSCode is not working very well with .vue files with TypeScript
export default {
  data() {
    return {
      name: '',
      submitted: false
    }
  },
  computed: {
    ...mapState('projects', ['current'])
  },
  methods: {
    ...mapActions('projects', ['createProject']),
    handleSubmit(e) {
      if (!this.name) {
        return
      }
      this.submitted = true
      const self = this
      this.createProject({name: self.name})
          .then((result) => {
            self.$router.push('/project-start')
            self.submitted = false
          })
          .catch((error) => {
            self.submitted = false
          })
    }
  }
}
</script>
