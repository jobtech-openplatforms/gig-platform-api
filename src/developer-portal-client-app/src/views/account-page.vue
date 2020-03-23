<template lang="pug">
  div.home
    div(v-if="ready")
      ContactPerson

</template>

<script lang="ts">
import ContactPerson from '../components/organisms/contact-person.vue'
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'

@Component({
  computed: {
  },
  data() {
    return {
      ready: false
    }
  },
  methods: {
    ...mapGetters('projects', ['currentProject'])
  },
  async created() {
    await this.$store.dispatch('projects/initCurrentProject')
    this.ready = true
  },
  components: {
    ContactPerson
  }
})
export default class HomePage extends Vue {}
</script>

