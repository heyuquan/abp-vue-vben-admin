import { useI18n } from '/@/hooks/web/useI18n';
import { FormProps, FormSchema } from '/@/components/Form';

const { t } = useI18n();

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 120,
    schemas: [
      {
        field: 'filter',
        component: 'Input',
        label: t('LeopardSaas.Search'),
        colProps: { span: 24 },
      },
    ],
  };
}

export function getModalFormSchemas(): FormSchema[] {
  return [
    {
      field: 'id',
      component: 'Input',
      label: 'id',
      show: false,
    },
    {
      field: 'name',
      component: 'Input',
      label: t('LeopardSaas.DisplayName:TenantName'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'adminEmailAddress',
      component: 'Input',
      label: t('LeopardSaas.DisplayName:AdminEmailAddress'),
      colProps: { span: 24 },
      required: true,
      ifShow: ({ values }) => {
        return values.id ? false : true;
      },
    },
    {
      field: 'adminPassword',
      component: 'InputPassword',
      label: t('LeopardSaas.DisplayName:AdminPassword'),
      colProps: { span: 24 },
      required: true,
      ifShow: ({ values }) => {
        return values.id ? false : true;
      },
    },
  ];
}

export function getConnectionFormSchemas(): FormSchema[] {
  return [
    {
      field: 'name',
      component: 'Input',
      label: t('LeopardSaas.DisplayName:Name'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'value',
      component: 'Input',
      label: t('LeopardSaas.DisplayName:Value'),
      colProps: { span: 24 },
      required: true,
    },
  ];
}
