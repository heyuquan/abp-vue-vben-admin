import { useI18n } from '/@/hooks/web/useI18n';
import { FormSchema } from '/@/components/Form';

const { t } = useI18n();

export function getOuFormSchemas(): FormSchema[] {
  return [
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
      field: 'displayName',
      component: 'Input',
      label: t('AbpIdentity.OrganizationUnits'),
      colProps: { span: 24 },
      required: true,
    },
  ];
}
