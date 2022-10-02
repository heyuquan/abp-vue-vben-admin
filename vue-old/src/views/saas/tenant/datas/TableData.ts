import { useI18n } from '/@/hooks/web/useI18n';
import { BasicColumn } from '/@/components/Table';

const { t } = useI18n();

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: t('LeopardSaas.DisplayName:TenantName'),
      dataIndex: 'name',
      align: 'left',
      width: 'auto',
      sorter: true,
    },
  ];
}

export function getConnectionStringsColumns(): BasicColumn[] {
  return [
    {
      title: t('LeopardSaas.DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 180,
      sorter: true,
    },
    {
      title: t('LeopardSaas.DisplayName:Value'),
      dataIndex: 'value',
      align: 'left',
      width: 'auto',
      sorter: true,
    },
  ];
}
