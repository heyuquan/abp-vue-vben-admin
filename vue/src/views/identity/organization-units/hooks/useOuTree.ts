import { computed, onMounted, ref } from 'vue';
import { cloneDeep } from 'lodash-es';
import { Modal } from 'ant-design-vue';
import { useI18n } from '/@/hooks/web/useI18n';
import { create, deleteById, getAll, move, update } from '/@/api/identity/organization-units';
import { listToTree } from '/@/utils/helper/treeHelper';

import { useOuModal } from './useOuModal';

export function useOuTree({ emit }: { emit: EmitType }) {
  const { t } = useI18n();
  const organizationUnitTree = ref([]);

  const { registerModal, openModal, formTitle, formSchemas, handleSubmit } = useOuModal();

  const getContentMenus = computed(() => {
    return (node: any) => {
      return [
        {
          label: t('AbpIdentity.Edit'),
          handler: () => {
            openModal(true, cloneDeep(node.$attrs), true);
          },
          icon: 'ant-design:edit-outlined',
        },
        {
          label: t('AbpIdentity.OrganizationUnit:AddChildren'),
          handler: () => {
            handleAddNew(node.$attrs.id);
            // openModal(true, { parentId: node.$attrs.id }, true);
          },
          icon: 'ant-design:plus-outlined',
        },
        {
          label: t('AbpIdentity.Delete'),
          handler: () => {
            Modal.warning({
              title: t('AbpIdentity.AreYouSure'),
              content: t('AbpIdentity.OrganizationUnit:WillDelete', [
                node.$attrs.displayName,
              ] as Recordable),
              okCancel: true,
              onOk: () => {
                deleteById(node.$attrs.id).then(() => {
                  loadTree();
                });
              },
            });
          },
          icon: 'ant-design:delete-outlined',
        },
      ];
    };
  });

  function loadTree() {
    getAll().then((res) => {
      organizationUnitTree.value = listToTree(res.items, {
        id: 'id',
        pid: 'parentId',
      });
    });
  }

  function handleAddNew(parentId?: string) {
    openModal(true, { parentId: parentId }, true);
  }

  function handleSelect(selectedKeys) {
    emit('select', selectedKeys[0]);
  }

  function handleDrop(opt) {
    const api =
      opt.dropPosition === -1
        ? move(opt.dragNode.eventKey) // parent
        : move(opt.dragNode.eventKey, opt.node.eventKey); // children
    api.then(() => loadTree());
  }

  onMounted(() => {
    loadTree();
  });
  return {
    organizationUnitTree,
    getContentMenus,
    handleDrop,
    handleAddNew,
    handleSelect,
    registerModal,
    formTitle,
    formSchemas,
    handleSubmit,
  };
}
