import { Component } from '@angular/core';
import { ShareModule } from '../../shared/share-module';
import { FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms';
import { TemplateListTablesGroupsFilter } from '../../models/master-data/template-list-tables-groups.model';
import { PaginationResult } from '../../models/base.model';
import { TEMPLATE_LIST_TABLES_RIGHTS } from '../../shared/constants';
import { TemplateListTablesGroupsService } from '../../services/master-data/template-list-tables-groups.service';
import { GlobalService } from '../../services/global.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ActivatedRoute, Router } from '@angular/router';
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-template-list-tables-groups',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './template-list-tables-groups.component.html',
  styleUrl: './template-list-tables-groups.component.scss'
})
export class TemplateListTablesGroupsComponent {
  validateForm: FormGroup = this.fb.group({
    code: [''],
    id: ['', [Validators.required]],
    name: ['', [Validators.required]],
    isActive: [true, [Validators.required]],
  })
  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new TemplateListTablesGroupsFilter()
  paginationResult = new PaginationResult()
  templateListTablesGroups: any[] = []
  loading: boolean = false
  TEMPLATE_LIST_TABLE_RIGHTS = TEMPLATE_LIST_TABLES_RIGHTS
  constructor(
    private _service: TemplateListTablesGroupsService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private message: NzMessageService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách template bảng biểu',
        path: 'master-data/template-list-tables-groups',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
  }

  ngOnDestroy() {
    this.globalService.setBreadcrumb([])
  }

  ngOnInit(): void {
    this.search()
  }

  onSortChange(name: string, value: any) {
    this.filter = {
      ...this.filter,
      SortColumn: name,
      IsDescending: value === 'descend',
    }
    this.search()
  }

  search() {
    this.isSubmit = false
    this._service.searchTemplateListTablesGroups(this.filter).subscribe({
      next: (data) => {
        this.templateListTablesGroups = data.data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }



  isCodeExist(id: string): boolean {
    return this.templateListTablesGroups?.some(
      (groups: any) => groups.id === id,
    )
  }

  submitForm(): void {
    this.isSubmit = true
    if (this.validateForm.valid) {
      const formData = this.validateForm.getRawValue()
      if (this.edit) {
        this._service.updateTemplateListTablesGroups(formData).subscribe({
          next: (data) => {
            this.search()
          },
          error: (response) => {
            console.log(response)
          },
        })
      } else {
        if (this.isCodeExist(formData.id)) {
          this.message.error(
            `Mã loại template bảng biểu ${formData.id} đã tồn tại, vui lòng nhập lại`,
          )
          return
        }
        formData.code = uuidv4();
        this._service.createTemplateListTablesGroups(formData).subscribe({
          next: (data) => {
            this.search()
          },
          error: (response) => {
            console.log(response)
          },
        })
      }
    } else {
      Object.values(this.validateForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty()
          control.updateValueAndValidity({ onlySelf: true })
        }
      })
    }
  }

  close() {
    this.visible = false
    this.resetForm()
  }

  reset() {
    this.filter = new TemplateListTablesGroupsFilter()
    this.search()
  }

  openCreate() {
    this.edit = false
    this.visible = true
  }

  resetForm() {
    this.validateForm.reset()
    this.isSubmit = false
  }

  deleteItem(code: string) {
    this._service.deleteTemplateListTablesGroups(code).subscribe({
      next: (data) => {
        this.search()
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  openEdit(data: {
    code: string
    id: string
    name: string
    isActive: boolean
  }) {
    this.validateForm.setValue({
      code: data.code,
      id: data.id,
      name: data.name,
      isActive: data.isActive,
    })
    setTimeout(() => {
      this.edit = true
      this.visible = true
    }, 200)
  }

  preview(data: { code: string }) {
    console.log(data);

    this._service.searchTemplateListTablesGroups({ KeyWord: data.code }).subscribe({
      next: (result) => {
        if (result.data && result.data.length > 0) {
          const templateListTable = result.data[0]
          this.router.navigate(['/master-data/template-list-tables', data.code], {
            state: { templateListTable },
          })
        }
      },
      error: (error) => console.error(error),
    })
  }

  pageSizeChange(size: number): void {
    this.filter.currentPage = 1
    this.filter.pageSize = size
    this.search()
  }

  pageIndexChange(index: number): void {
    this.filter.currentPage = index
    this.search()
  }
}
