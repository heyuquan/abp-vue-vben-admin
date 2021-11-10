import { computed, onMounted, ref } from 'vue';
import { cloneDeep } from 'lodash-es';
import { Modal } from 'ant-design-vue';
import { useI18n } from '/@/hooks/web/useI18n';
import { FormSchema } from '/@/components/Form';
import { create, deleteById, getAll, move, update } from '/@/api/identity/organization-units';
import { listToTree } from '/@/utils/helper/treeHelper';
import { useModal } from '/@/components/Modal';

export function useOuTree({ emit }: { emit: EmitType }) {
  const { t } = useI18n();
  const organizationUnitTree = ref([]);

  const formSchemas: FormSchema[] = [
    {
      field: 'id',
      component: 'Input',
      label: 'id',
      colProps: { span: 24 },
      ifShow: false,
    },
    {
      field: 'parentId',
      component: 'Input',
      label: 'parentId',
      colProps: { span: 24 },
      ifShow: false,
    },
    {
      field: 'parentDisplayName',
      component: 'Input',
      label: t('AbpIdentity.OrganizationUnit:Parent'),
      colProps: { span: 24 },
      ifShow: ({ values }) => {
        debugger;
        return values.parentId ? true : false;
      },
      dynamicDisabled: true,
    },
    {
      field: 'displayName',
      component: 'Input',
      label: t('AbpIdentity.OrganizationUnit:DisplayName'),
      colProps: { span: 24 },
      required: true,
    },
  ];

  const [registerModal, { openModal }] = useModal();

  const getContentMenus = computed(() => {
    return (node: any) => {
      return [
        {
          label: t('AbpIdentity.Edit'),
          handler: () => {
            debugger;
            openModal(true, cloneDeep(node.$attrs), true);
          },
          icon: 'ant-design:edit-outlined',
        },
        {
          label: t('AbpIdentity.OrganizationUnit:AddChildren'),
          handler: () => {
            handleAddChildren(node.$attrs.id, node.$attrs.displayName);
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

  function handleAddChildren(parentId?: string, parentDisplayName?: string) {
    openModal(true, { parentId: parentId, parentDisplayName: parentDisplayName }, true);
  }

  function handleAddNew() {
    openModal(true);
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

  function handleSubmit(val) {
    const api = val.id
      ? update(val.id, {
          displayName: val.displayName,
        })
      : create(val);
    return api.then(() => loadTree());
  }

  onMounted(() => {
    loadTree();
  });
  return {
    organizationUnitTree,
    getContentMenus,
    registerModal,
    formSchemas,
    handleDrop,
    handleAddNew,
    handleSelect,
    handleSubmit,
  };
}
