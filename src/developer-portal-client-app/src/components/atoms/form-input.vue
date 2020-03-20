<template lang="pug">
div.form-input
  label {{label}}
    input(
      :type="type"
      class="form-control"
      :class="cssClasses"
      :placeholder="placeholder"
      :value=value
      @onChange=onChange
      )
  slot
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'

@Component
export default class FormInput extends Vue {
  @Prop({ default: 'text' }) public type!: string
  @Prop({}) public value!: string
  @Prop({ default: '' }) public label!: string
  @Prop({ default: '' }) public placeholder!: string
  @Prop({ default: '' }) public cssClasses!: string
  public onChange() {
    this.$emit('change', this.value)
  }
}
</script>

<style lang="stylus" scoped>
</style>
