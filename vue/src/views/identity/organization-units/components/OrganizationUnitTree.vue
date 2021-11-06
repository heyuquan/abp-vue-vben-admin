<template>
  <Card :title="t('AbpIdentity.OrganizationUnit:OrganizationTree')">
    <template #extra>
      <a-button
        v-if="hasPermission('LeopardIdentity.OrganizationUnits.Create')"
        type="primary"
        pre-icon="ant-design:plus-outlined"
        @click="handleAddNew"
      >
        {{ t('AbpIdentity.OrganizationUnit:AddRootUnit') }}
      </a-button>
    </template>
    <BasicTree
      ref="treeElRef"
      :draggable="true"
      :click-row-to-expand="false"
      :tree-data="organizationUnitTree"
      :replace-fields="{
        title: 'displayName',
        key: 'id',
      }"
      :before-right-click="getContentMenus"
      defaultExpandLevel="1"
      @select="handleSelect"
      @drop="handleDrop"
    />
    <BasicModal v-bind="$attrs" @register="registerModal" @ok="handleSubmit" :title="formTitle">
      <BasicForm
        ref="formElRef"
        :colon="true"
        :schemas="formSchemas"
        :label-width="120"
        :show-action-button-group="false"
        :action-col-options="{
          span: 24,
        }"
      />
    </BasicModal>
  </Card>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import { Card } from 'ant-design-vue';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { BasicTree } from '/@/components/Tree';
  import { useOuTree } from '../hooks/useOuTree';

  export default defineComponent({
    name: 'OrganizationUnitTree',
    components: { BasicTree, Card },
    emits: ['change', 'select'],
    setup(_props, { emit }) {
      const { t } = useI18n();
      const {
        organizationUnitTree,
        getContentMenus,
        handleDrop,
        handleAddNew,
        handleSelect,
        registerModal,
        formTitle,
        formSchemas,
        handleSubmit,
      } = useOuTree({ emit });

      const { hasPermission } = usePermission();

      return {
        t,
        organizationUnitTree,
        getContentMenus,
        hasPermission,
        handleDrop,
        handleAddNew,
        handleSelect,
        registerModal,
        formTitle,
        formSchemas,
        handleSubmit,
      };
    },
  });
</script>
