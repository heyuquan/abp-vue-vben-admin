import type { Ref } from 'vue';

import { computed, unref, watch } from 'vue';
import { useI18n } from '/@/hooks/web/useI18n';
import { FormActionType } from '/@/components/Form';
import { getOuFormSchemas } from '../datas/ModalData';
import { create, update } from '/@/api/identity/organization-units';
import { useModal } from '/@/components/Modal';

export function useOuModal() {
  const { t } = useI18n();
  const formTitle = t('AbpIdentity.OrganizationUnit:NewOrganizationUnit');

  const [registerModal, { openModal }] = useModal();

  const formSchemas = computed(() => {
    return [...getOuFormSchemas()];
  });

  function handleSubmit(input) {
    const api = input.id
      ? update(input.id, {
          displayName: input.displayName,
        })
      : create(input);
    return api;
  }

  return {
    registerModal,
    openModal,
    formTitle,
    formSchemas,
    handleSubmit,
  };
}
