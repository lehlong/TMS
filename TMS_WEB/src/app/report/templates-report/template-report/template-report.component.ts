import { Component, TemplateRef } from '@angular/core';
import { ShareModule } from '../../../shared/share-module';
import { DropdownService } from '../../../services/dropdown/dropdown.service';
import { environment } from '../../../../environments/environment';
import { v4 as uuidv4 } from 'uuid';
import { NzUploadXHRArgs } from 'ng-zorro-antd/upload';
import { Subscription } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ReportService } from '../../../services/report/report.service';
import { PaginationResult } from '../../../models/base.model';
import { FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms';
import { NzFormatEmitEvent } from 'ng-zorro-antd/tree';
import { OpinionListService } from '../../../services/business/opinion-list.service';
import { OrganizeService } from '../../../services/system-manager/organize.service';
import { AuditPeriodListTablesService } from '../../../services/master-data/audit-period-list-tables.service';

interface SelectedData {
  orgCode: string;
  opinionCode: string[];
}
@Component({
  selector: 'app-template-report',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './template-report.component.html',
  styleUrl: './template-report.component.scss'
})
export class TemplateReportComponent {
  validateForm: FormGroup = this.fb.group({
    fileId: ['', [Validators.required]],
    textElement: ['', [Validators.required]],
    typeElement: ['', [Validators.required]],
    valueInput: ['', [Validators.required]],
    orgCode: [''],
    opinionCode: [''],
    tableCode: [''],
    isActive: [true, [Validators.required]],
  })
  suffixIcon: string | TemplateRef<void> | undefined
  listTimeYear: any[] = [];
  listAudit: any[] = [];
  yearValue: string = ''
  auditValue: string = ''
  listElement: any[] = [];
  isVisible = false;
  public baseUrl = environment.baseApiUrl
  referenceId = '';
  paginationResult: any[] = []
  listOfItems: { [key: string]: string } = {
    '1': 'FreeText',
    '2': 'Kiến Nghị',
    '3': 'Bảng biểu'
  };
  selectedItem = '';
  nodeOrg: any[] = []
  searchValueOrg = ''
  nodeOpinion: any[] = []
  searchValueOpinion = ''
  visible: boolean = false
  nodeCurrentOrg!: any
  listOfTables: any[] = [];
  valueInput: string = '';
  textElement: string = '';
  fileId: string = '';
  selectedTableCode: string = '';
  selectedOrgNodes: any[] = [];
  selectedOpinionNodes: { [orgCode: string]: string[] } = {};
  currentOrgCode: string = '';
  constructor(
    private dropDownService: DropdownService,
    private http: HttpClient,
    private _service: ReportService,
    private fb: NonNullableFormBuilder,
    private _serviceOpinionList: OpinionListService,
    private _serviceOrg: OrganizeService,
    private _tableService: AuditPeriodListTablesService,

  ) { }

  ngOnInit(): void {
    this.getTimeYear();
    this.getAuditPeriod();
    this.generateNewReferenceId();
   // this.getNodeOrg();
  }
  
  keys(object: any): string[] {
    return Object.keys(object);
  }
  customRequest = (item: NzUploadXHRArgs): Subscription => {
    const formData = new FormData();
    formData.append('file', item.file as any);
    formData.append('referenceId', this.referenceId || '');
    formData.append('yearValue', this.yearValue || '');
    formData.append('auditValue', this.auditValue || '');
    let params = new HttpParams();
    if (this.referenceId && this.yearValue && this.auditValue) {
      params = params.set('referenceId', this.referenceId);
      params = params.set('yearValue', this.yearValue);
      params = params.set('auditValue', this.auditValue);
    }
    return this.http.post(this.baseUrl + '/Report/UploadTemplate', formData, {
      params: params,
    }).subscribe({
      next: (res: any) => {
        console.log(res)
      },
    });
  }

  getTimeYear() {
    this.dropDownService.getAllPeriodTime().subscribe({
      next: (data) => {
        this.listTimeYear = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  getAuditPeriod() {
    this.dropDownService.getAllAudit().subscribe({
      next: (data) => {
        this.listAudit = data
      },
      error: (response) => {
        console.log(response)
      }
    })
  }
  generateNewReferenceId(): void {
    this.referenceId = uuidv4();
  }
  getNodeOrg() {
    this._service.getListOrg(this.fileId,this.textElement).subscribe((res) => {
      this.nodeOrg = [res]
      console.log(this.nodeOrg)
    })
  }

  getListTemplate(yearValue: string, auditValue: string) {
    this._service.getListTemplate(yearValue, auditValue).subscribe({
      next: (data) => {
        this.paginationResult = data
        this.listElement = []
      },
      error: (response) => {
        console.log(response)
      }
    })
  }
  openDetail(id: string) {
    this._service.getListElement(id).subscribe({
      next: (data) => {
        this.listElement = data      
      },
      error: (response) => {
        console.log(response)
      }
    })
  }
  openModal(textElement: string, fileId : string) {
    this.isVisible = true;
    this.textElement = textElement;
    this.fileId = fileId;
    this.getNodeOrg()
  }
  handleOk(): void {
    this.isVisible = false;
    const selectedData = this.gatherSelectedData();
    const data = {
      textElement: this.textElement,
      type: this.selectedItem,
      valueInput: this.valueInput,
      fileId: this.fileId,
      tableCode: this.selectedItem === '3' ? this.selectedTableCode : '',
      orgCode: this.selectedItem === '2' ? selectedData[0].orgCode : '',
      opinionCode: this.selectedItem === '2' ? selectedData[0].opinionCode : [],
    }
    this._service.SaveTemplateReport(data).subscribe({
      next: (data) => {
      },
      
    })
  }
  onTableSelect(tableCode: string): void {
    this.selectedTableCode = tableCode;
  }

  handleCancel(): void {
    this.isVisible = false;
    this.selectedItem = '';
  }
  ItemChange(value: string): void {
    this.getTables()
    
  }
  nzCheck(event: NzFormatEmitEvent): void {
    console.log(event)
    if (event.node?.origin['pId'] == 'ORG') {  // Nếu là nút gốc (đơn vị)
      this.updateSelectedOrgNodes(event.node.origin, event.node.origin.checked!);
    } else { 
      this.updateSelectedOpinionNodes(this.currentOrgCode, event.node?.origin['code'], event.node?.origin.checked!);
    }
  }
  updateSelectedOrgNodes(node: any, checked: boolean): void {
    if (checked) {
      this.selectedOrgNodes.push(node);
    } else {
      this.selectedOrgNodes = this.selectedOrgNodes.filter(n => n.key !== node.key);
    }
  }
  updateSelectedOpinionNodes(orgCode: string, opinionCode: string, checked: boolean): void {
    if (!this.selectedOpinionNodes[orgCode]) {
      this.selectedOpinionNodes[orgCode] = [];
    }
    
    if (checked) {
      if (!this.selectedOpinionNodes[orgCode].includes(opinionCode)) {
        this.selectedOpinionNodes[orgCode].push(opinionCode);
      }
    } else {
      this.selectedOpinionNodes[orgCode] = this.selectedOpinionNodes[orgCode].filter(code => code !== opinionCode);
    }
  }
  gatherSelectedData(): SelectedData[] {
    return this.selectedOrgNodes.map(orgNode => ({
      orgCode: this.currentOrgCode,
      opinionCode: this.selectedOpinionNodes[orgNode.key] || []
    }));
  }

  preCheckOpinions(orgCode: string): void {
    const selectedOpinions = this.selectedOpinionNodes[orgCode] || [];
    this.nodeOpinion.forEach(node => this.setNodeCheckedState(node, selectedOpinions));
  }

  setNodeCheckedState(node: any, selectedOpinions: string[]): void {
    if (selectedOpinions.includes(node.key)) {
      node.checked = true;
    }
    if (node.children) {
      node.children.forEach((child: any) => this.setNodeCheckedState(child, selectedOpinions));
    }
  }
  getTables(): void {
    this._tableService
      .GetMgListTablesByAuditPeriodCode(this.auditValue)
      .subscribe((response) => {
        this.listOfTables = response
        console.log(this.listOfTables) // Load data into the table
      })
  }
  onClickNodeOrg(node: any) {
    this.visible = true
    this.nodeCurrentOrg = node?.origin
    this.currentOrgCode = node?.origin.id;
    var data = {
      fileId: this.fileId,
      textElement :this.textElement,
      orgCode: node?.origin.id,
      timeYear: this.yearValue,
      auditPeriod: this.auditValue
    }
    this._service
    .getListOpinion(data)
    .subscribe((res) => {
      this.nodeOpinion = [res]
      this.preCheckOpinions(this.currentOrgCode);
    })
  }
}

