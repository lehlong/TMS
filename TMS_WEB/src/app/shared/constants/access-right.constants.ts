// Danh mục
export enum UNIT_RIGHTS {
  LIST = 'R1.2.1',
  EXCEL = 'R0',
  ADD = 'R1.2.2',
  EDIT = 'R1.2.2',
  DELETE = 'R0',
}

export enum CURRENCY_RIGHTS {
  LIST = 'R1.1.1',
  EXCEL = 'R0',
  ADD = 'R1.1.2',
  EDIT = 'R1.1.2',
  DELETE = 'R0',
}

export enum TYPE_OF_GOODS {
  LIST = 'R1.1.1',
  EXCEL = 'R0',
  ADD = 'R1.1.2',
  EDIT = 'R1.1.2',
  DELETE = 'R0',
}

export enum MARKET_RIGHTS {
  LIST = 'R1.1.1',
  EXCEL = 'R0',
  ADD = 'R1.1.2',
  EDIT = 'R1.1.2',
  DELETE = 'R0',
}

export enum RETAIL_PRICE_RIGHTS {
  LIST = 'R1.1.1',
  EXCEL = 'R0',
  ADD = 'R1.1.2',
  EDIT = 'R1.1.2',
  DELETE = 'R0',
}
export enum LOCAL_RIGHTS {
  LIST = 'R1.4.1',
  EXCEL = 'R0',
  ADD = 'R1.4.2',
  EDIT = 'R1.4.2',
  DELETE = 'R0',
}

export enum AREA_RIGHTS {
  LIST = 'R1.3.1',
  EXCEL = 'R0',
  ADD = 'R1.3.2',
  EDIT = 'R1.3.2',
  DELETE = 'R0',
}

export enum OPINIONTYPE_RIGHTS {
  LIST = 'R1.5.1',
  EXCEL = 'R0',
  ADD = 'R1.5.2',
  EDIT = 'R1.5.2',
  DELETE = 'R0',
}
export enum TEMPLATE_LIST_TABLES_RIGHTS {
  LIST = 'R5.1',
  EXCEL = 'R0',
  ADD = 'R5.2',
  EDIT = 'R5.3',
  DELETE = 'R0',
}
export enum PERIODTIME_RIGHTS {
  LIST = 'R1.7.1',
  EXCEL = 'R0',
  ADD = 'R1.7.2',
  EDIT = 'R1.7.2',
  DELETE = 'R0',
}
export enum AUDITPERIOD_RIGHTS {
  LIST = 'R1.8.1',
  EXCEL = 'R0',
  ADD = 'R1.8.2',
  EDIT = 'R1.8.2',
  DELETE = 'R0',
}
export enum MGLISTTABLE_RIGHTS {
  LIST = 'R3.1',
  EXCEL = 'R0',
  ADD = 'R3.2',
  EDIT = 'R3.3',
  DELETE = 'R0',
  EDIT_TREE = 'R3.5',
  ADD_TREE = 'R3.4',
}
export enum MGOPINIONLIST_RIGHTS {
  LIST = 'R4.1',
  EXCEL = 'R0',
  ADD = 'R4.2',
  EDIT = 'R4.3',
  DELETE = 'R0',
  EDIT_TREE = 'R4.5',
  ADD_TREE = 'R4.4',
  EDIT_PENDING = 'R4.6',
}
export enum ACCOUNTTYPE_RIGHTS {
  LIST = 'R1.6.1',
  EXCEL = 'R0',
  ADD = 'R1.6.2',
  EDIT = 'R1.6.2',
  DELETE = 'R0',
}

export enum AUDITPERIODLISTTABLES_RIGHTS {
  LIST = 'R4',
  EXCEL = 'R4',
  ADD = 'R4',
  EDIT = 'R4',
  DELETE = 'R4',
  PHE_DUYET = 'R4.1',
  HUY_PHE_DUYET = 'R4.4',
  TU_CHOI = 'R4.2',
  HUY_TRINH_DUYET = 'R4.3',
}

// Hệ thống

export enum ACCOUNT_GROUP_RIGHTS {
  LIST = 'R2.5',
  EXCEL = 'R2.5',
  ADD = 'R2.5',
  EDIT = 'R2.5',
  DELETE = 'R2.5',
}

// Quyền Đợt kiểm toán

export enum LIST_AUDIT_RIGHTS {
  LIST = 'RP1.1',
  EXCEL = 'RP1.1',
  ADD = 'RP1.1.1',
  EDIT = 'RP1.1.7',
  DELETE = 'R0',
  PHE_DUYET = 'RP1.1.3',
  HUY_PHE_DUYET = 'RP1.1.4',
  TU_CHOI = 'RP1.1.6',
  YEU_CAU_CHINH_SUA = 'RP1.1.5',
  TRINH_DUYET = 'RP1.1.2',
  HUY_TRINH_DUYET = 'RP1.1.8',
}
export enum LIST_AUDIT_OPINION_RIGHTS {
  LIST = 'RP1.2.1',
  EXCEL = 'RP1.2.1',
  // ADD = 'RP1.1.1',
  // EDIT = 'RP1.1.7',
  // DELETE = 'R0',
  PHE_DUYET = 'RP1.2.2',
  HUY_PHE_DUYET = 'RP1.2.3',
  XAC_NHAN_HOAN_THANH = 'RP1.2.4',
  XAC_NHAN_CHUA_HOAN_THANH = 'RP1.2.5',
  TU_CHOI = 'RP1.2.6',
  CAP_NHAT_NOI_DUNG_GIAI_TRINH = 'RP1.2.7',
  XEM_NOI_DUNG_GIAI_TRINH = 'RP1.2.11',
  NOI_DUNG_GIAI_TRINH_CUA_DON_VI = 'RP1.2.9',
  NHAP_NOI_DUNG_GIAI_TRINH_CUA_DON_VI = 'RP1.2.10',
  TRINH_DUYET = 'RP1.2.8',
  // HUY_TRINH_DUYET = 'RP1.1.8',
}

export enum LIST_AUDIT_TABLE_RIGHTS {
  LIST = 'RP1.3',
  EXCEL = 'RP1.3',
  ADD = 'RP1.3.1',
  EDIT = 'RP1.3.4',
  EDIT_KIEM_TOAN = 'RP1.3.4.1',
  EDIT_GIAI_TRINH = 'RP1.3.4.2',
  XEM_CHI_TIET = 'RP1.3.3',
  XEM_TAT_CA = 'RP1.3.3.1',
  XEM_THEO_DON_VI = 'RP1.3.3.2',
  DELETE = 'RP1.3.2',
  TRINH_DUYET = 'RP1.3.5',
  PHE_DUYET = 'RP1.3.6',
  STC_PHE_DUYET = 'RP1.3.6.1',
  XAC_NHAN = 'RP1.3.6.2',
  HUY_PHE_DUYET = 'RP1.3.7',
  TU_CHOI = 'RP1.3.8',
  STC_TU_CHOI = 'RP1.3.8.1',
  TONG_TU_CHOI = 'RP1.3.8.2',
  HUY_TRINH_DUYET = 'RP1.3.9',
  EXPORT = 'RP1.3.10',
  EXPORT_ALL = 'RP1.3.10.1',
  EXPORT_ORG = 'RP1.3.10.2',
}

// Nghiệp vụ

// export enum ORDER_NPP_RIGHTS {
//   LIST = 'R3.1.1',
//   EXCEL = 'R3.1.2',
//   ADD = 'R3.1.3',
//   EDIT = 'R3.1.4',
//   DETAIL = 'R3.1.5',
//   DELETE = 'R3.1.6',
//   CANCEL = 'R3.1.7',
//   SEND = 'R3.1.8',
// }

// export enum ORDER_C2_RIGHTS {
//   LIST = 'R3.2.1',
//   EXCEL = 'R3.2.2',
//   ADD = 'R3.2.3',
//   FORWARD = 'R3.1.3',
//   EDIT = 'R3.2.4',
//   DETAIL = 'R3.2.5',
//   DELETE = 'R3.2.6',
//   CANCEL = 'R3.2.7',
//   SEND = 'R3.2.8',
//   REFUSE = 'R3.2.9',
//   CONFIRM = 'R3.2.10',
//   RELEASE = 'R3.2.11',
// }

// export enum ORDER_C3_RIGHTS {
//   LIST = 'R3.3.1',
//   EXCEL = 'R3.3.2',
//   ADD = 'R3.3.3',
//   FORWARD = 'R3.2.3',
//   EDIT = 'R3.3.4',
//   DETAIL = 'R3.3.5',
//   DELETE = 'R3.3.6',
//   CANCEL = 'R3.3.7',
//   SEND = 'R3.3.8',
//   REFUSE = 'R3.3.9',
//   CONFIRM = 'R3.3.10',
//   RELEASE = 'R3.3.11',
// }

// export enum ACCOUNT_SALE_OFFICE {
//   LIST = 'R4.4.1',
//   EXCEL = 'R4.4.2',
//   EDIT = 'R4.4.3',
// }

// export enum PARTNER_RIGHTS {
//   LIST = 'R4',
//   EXCEL = 'R4',
//   ADD = 'R4',
//   EDIT = 'R4',
// }

// export enum RFID_RIGHTS {
//   LIST = 'R4',
//   EXCEL = 'R4',
//   ADD = 'R4',
//   EDIT = 'R4',
//   DELETE = 'R4',
// }

// export enum TYPE_PRODUCT {
//   LIST = 'R4',
//   EXCEL = 'R4',
//   ADD = 'R4',
//   EDIT = 'R4',
//   DELETE = 'R4',
// }

// export enum DRIVER_RIGHTS {
//   LIST = 'R4',
//   EXCEL = 'R4',
//   ADD = 'R4',
//   EDIT = 'R4',
//   DELETE = 'R4',
// }
