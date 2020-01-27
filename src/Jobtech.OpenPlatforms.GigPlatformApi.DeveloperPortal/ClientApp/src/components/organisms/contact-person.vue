<template lang="pug">
  .contact-person
    h2 Contact person
    .card
      form( @submit.prevent="handleSubmit")
        label(for="contact-person-name")
          span.label-text Name
          ValidationProvider(name="Name" rules="required" v-slot="{ errors }")
            input(type="text" id="contact-person-name" required v-model="contact.name" name="contactname" placeholder="Enter your name")
            .feedback
              .invalid-feedback {{ errors[0] }}
        label(for="contact-person-email")
          span.label-text Email
          ValidationProvider(name="Email" rules="required|email" v-slot="{ errors }")
            input(type="email" id="contact-person-email" required  v-model="contact.email" name="contactemail" placeholder="name@example.com")
            .feedback
              .invalid-feedback {{ errors[0] }}
        button.btn.btn-project.btn-right Save
</template>

<script lang="ts">
import { mapState, mapActions } from 'vuex'

export default {
    data() {
        return {
            contact: {
                name: '',
                email: '',
            },
            submitted: false
        }
    },
    computed: {
        ...mapState('account', ['status', 'user'])
    },
    created() {
      if (this.user) {
        this.contact.name = this.user.name
        this.contact.email = this.user.email
      }
    },
    methods: {
        ...mapActions('account', ['updateContact']),
        handleSubmit(e) {
            this.submitted = true
            this.updateContact(Object.assign(this.contact, { id: this.user.id }))
            this.submitted = false
        }
    }
}
</script>
