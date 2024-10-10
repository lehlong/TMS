import {
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core'
import { ShareModule } from '../../../shared/share-module'
import {
  NzFormatEmitEvent,
  NzTreeComponent,
  NzTreeNode,
  NzTreeNodeOptions,
} from 'ng-zorro-antd/tree'
import { TEMPLATE_LIST_TABLES_RIGHTS } from '../../../shared/constants'
import { TemplateListTablesDataService } from '../../../services/master-data/template-list-tables-data.service'
import { NzMessageService } from 'ng-zorro-antd/message'
import { ActivatedRoute, Router } from '@angular/router'
import { TemplateListTablesDataFilter } from '../../../models/master-data/template-list-tables-data.model'

@Component({
  selector: 'app-template-list-tables-edit',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './template-list-tables-edit.component.html',
  styleUrl: './template-list-tables-edit.component.scss',
})
export class TemplateListTablesEditComponent {
  @Input() temDetail!: any
  @Output() childEvent = new EventEmitter<any>()
  @Output() dataUpdated = new EventEmitter<string>();

  @ViewChild('treeOrganize') treeOrganize!: NzTreeComponent
  @ViewChild('treeListTables') treeListTables!: NzTreeComponent
  organizeNodes: any[] = []
  listTablesNodes: any[] = []
  loading: boolean = false
  searchValue: string = ''
  searchValueListTables: string = ''
  previouslyCheckedOrgs: string[] = []
  newlyCheckedOrg: string | null = null
  TEMPLATE_LIST_TABLES_RIGHTS = TEMPLATE_LIST_TABLES_RIGHTS
  filter = new TemplateListTablesDataFilter()
  selectedOrgCode: string | null = null
  currentSelectedOrgCode: string | null = null
  originalListTablesNodes: any[] = []
  originalOrganizeNodes: any[] = []
  allCheckedNodes: Set<string> = new Set();
  constructor(
    private templateListTablesDataService: TemplateListTablesDataService,
    private route: ActivatedRoute,
    private router: Router,
    private message: NzMessageService,
  ) { }
  ngOnInit() {
    // Khởi tạo các organize đã được chọn trước đó
    this.previouslyCheckedOrgs = this.temDetail.treeOrganize.children
      .filter((org: any) => org.isChecked)
      .map((org: any) => org.id)
    this.loadInit()

    this.originalListTablesNodes = [...this.listTablesNodes]
    this.originalOrganizeNodes = [...this.organizeNodes]
  }
  nzEvent(event: NzFormatEmitEvent): void { }
  initializeAllCheckedNodes() {
    this.allCheckedNodes = new Set(
      this.listTablesNodes
        .flatMap(node => this.getAllCheckedNodesRecursive(node))
        .map(node => node.code)
    );
  }
  getAllCheckedNodesRecursive(node: any): any[] {
    let checkedNodes: any[] = [];
    if (node.checked) {
      checkedNodes.push(node);
    }
    if (node.children) {
      node.children.forEach((child: any) => {
        checkedNodes = checkedNodes.concat(this.getAllCheckedNodesRecursive(child));
      });
    }
    return checkedNodes;
  }
  loadInit() {
    this.listTablesNodes = this.mapTreeNodes(this.temDetail.treeListTables)
    this.organizeNodes = this.mapTreeNodes(this.temDetail.treeOrganize)
    this.expandAllNodes(this.listTablesNodes)
    this.expandAllNodes(this.organizeNodes)
    this.initializeAllCheckedNodes()
  }
  onClickNode(event: NzFormatEmitEvent) {
    this.onTitleClick(event.node)
    this.onOrganizeNodeClick(event)
  }
  onOrganizeNodeClick(event: NzFormatEmitEvent): void {
    //Khi click vào một organize, cập nhật danh sách (opinionNodes) tương ứng.
    if (event?.node) {
      // Sử dụng optional chaining
      try {
        this.selectedOrgCode = event.node.key
        this.updateListTablesList(event.node)
      } catch (error) {
        console.error('Error in updateListTablesList:', error)
      }
    } else {
      console.log('Không có node để cập nhật')
    }
  }

  updateListTablesList(orgNode: NzTreeNode): void {
    if (!orgNode) return

    // Tìm dữ liệu của organize node từ dữ liệu gốc
    const orgData = this.findOrgDataById(
      this.temDetail.treeOrganize.children,
      orgNode.key,
    )

    if (orgData && orgData.treeListTables) {
      this.listTablesNodes = this.mapTreeNodes(orgData.treeListTables)
    } else {
      this.listTablesNodes = this.mapTreeNodes(this.temDetail.treeListTables)
    }

    this.updateListTablesCheckedStatus(
      this.listTablesNodes,
      orgData ? orgData.treeListTables : [],
    )

    if (this.treeListTables) {
      this.treeListTables.nzTreeService.initTree(this.listTablesNodes)
      this.expandAllNodes(this.listTablesNodes)
    }
  }

  findOrgDataById(nodes: any[], id: string): any {
    if (!nodes) return null
    for (const node of nodes) {
      if (node.id === id) {
        return node
      }
      if (node.children) {
        const found = this.findOrgDataById(node.children, id)
        if (found) return found
      }
    }
    return null
  }

  updateListTablesCheckedStatus(nodes: any[], dbOpinions: any[]): void {
    nodes.forEach((node) => {
      const dbOpinion = dbOpinions.find((op: any) => op.id === node.key)
      if (dbOpinion) {
        node.checked = dbOpinion ? dbOpinion.isChecked : false
      }
      if (node.children) {
        this.updateListTablesCheckedStatus(node.children, dbOpinions)
      }
    })
  }

  mapTreeNodes(data: any): any[] {
    if (!data) return []

    // If it's already an array, map each node
    if (Array.isArray(data)) {
      return data.map((node) => this.createTreeNode(node))
    }

    // If it's a single node (like the root), wrap it in an array
    return [this.createTreeNode(data)]
  }

  createTreeNode(node: any): any {
    if (!node) return null

    const mappedNode = {
      code: node.code,
      id: node.id,
      name: node.name,
      pId: node.pId,
      title: node.title || `${node.id}_${node.name}`,
      key: node.id,
      checked: node.isChecked,
      isLeaf: !node.children || node.children.length === 0,
      mgCode: node.mgCode,
      children: node.children ? this.mapTreeNodes(node.children) : [],
    }

    return mappedNode
  }
  onCheckboxChange(node: any): void {
    this.toggleNodeAndChildren(node, node.checked)
    this.updateChildrenNodes(node, node.isChecked);
    this.updateParentNodes(node)
  }

  toggleNodeAndChildren(node: any, checked: boolean): void {
    node.checked = checked
    if (node.children) {
      node.children.forEach((child: any) =>
        this.toggleNodeAndChildren(child, checked),
      )
    }
  }

  updateNodeCheckStatus(node: NzTreeNode): void {
    node.isChecked = !node.isChecked;
  }

  updateChildrenNodes(node: NzTreeNode, checked: boolean): void {
    if (node.children) {
      node.children.forEach(child => {
        child.isChecked = checked;
        this.updateChildrenNodes(child, checked);
      });
    }
  }

  updateParentNodes(node: NzTreeNode): void {
    const parentNode = node.getParentNode();
    if (parentNode) {
      const allChecked = parentNode.children.every(child => child.isChecked);
      const someChecked = parentNode.children.some(child => child.isChecked);

      parentNode.isChecked = allChecked;
      parentNode.isHalfChecked = someChecked && !allChecked;

      this.updateParentNodes(parentNode);
    }
  }
  findParentNode(nodes: any[], targetNode: any): any {
    for (const node of nodes) {
      if (node.children && node.children.includes(targetNode)) {
        return node
      }
      if (node.children) {
        const foundParent = this.findParentNode(node.children, targetNode)
        if (foundParent) return foundParent
      }
    }
    return null
  }
  expandAllNodes(nodes: any[]): void {
    //Mở rộng tất cả các node trong cây để chúng hiển thị hết.
    nodes.forEach((node) => {
      node.expanded = true
      if (node.children && node.children.length > 0) {
        this.expandAllNodes(node.children)
      }
    })
  }
  getAllCheckedNodes(nodes: NzTreeNode[]): NzTreeNode[] {
    let checkedNodes: NzTreeNode[] = []
    for (let node of nodes) {
      // Exclude the root node
      if (
        (node.isChecked && node.key !== 'LTB' && node.key !== 'ORG') ||
        (node.isHalfChecked && node.key !== 'LTB' && node.key !== 'ORG')
      ) {
        checkedNodes.push(node)
      }
      if (node.children) {
        checkedNodes = checkedNodes.concat(
          this.getAllCheckedNodes(node.children),
        )
      }
    }
    return checkedNodes
  }
  onOrganizeCheckChange(event: NzFormatEmitEvent): void {
    const checkedNode = event.node!
    if (this.previouslyCheckedOrgs.includes(checkedNode.key)) {
      if (!checkedNode.isChecked) {
        checkedNode.isChecked = true
        this.treeOrganize.nzTreeService.setCheckedNodeList(checkedNode)
        return
      }
    }
    if (checkedNode.isChecked) {
      if (!this.previouslyCheckedOrgs.includes(checkedNode.key)) {
        if (this.newlyCheckedOrg) {
          const previousNewNode = this.treeOrganize.getTreeNodeByKey(
            this.newlyCheckedOrg,
          )
          if (previousNewNode) {
            previousNewNode.isChecked = false
            this.treeOrganize.nzTreeService.setCheckedNodeList(previousNewNode)
          }
        }
        this.newlyCheckedOrg = checkedNode.key
      }
    } else {
      if (checkedNode.key === this.newlyCheckedOrg) {
        this.newlyCheckedOrg = null
      }
    }

    this.treeOrganize.nzTreeService.setCheckedNodeList(checkedNode)
  }
  getExistingOpinions(orgCode: string): any[] {
    const org = this.temDetail.treeOrganize.children.find(
      (org: any) => org.id === orgCode,
    )
    const getCheckedListTables = (listTables: any[]): any[] => {
      let result: any[] = []
      listTables.forEach((op) => {
        if (op.isChecked) {
          result.push({
            code: op.code,
            id: op.id,
            title: op.title,
            isChecked: op.isChecked,
          })
        }
        if (op.children && op.children.length > 0) {
          result = result.concat(getCheckedListTables(op.children))
        }
      })
      return result
    }

    if (org && org.treeListTables) {
      return getCheckedListTables(org.treeListTables)
    }
    return []
  }

  updateLocalData(allCheckedCodes: string[]): void {
    const remainingListTablesCodes = new Set(allCheckedCodes);

    const updateNodeStatus = (nodes: any[]) => {
      nodes.forEach((node) => {
        if (node.code) {
          node.checked = remainingListTablesCodes.has(node.code);
        }
        if (node.children && node.children.length > 0) {
          updateNodeStatus(node.children);
        }
      });
    };

    updateNodeStatus(this.listTablesNodes);
    this.treeListTables.getTreeNodes().forEach(node => {
      this.treeListTables.getTreeNodeByKey(node.key)?.setChecked(node.isChecked);
    });
  }
  sendDataToParent() {
    const organizeChecked = this.getAllCheckedNodes(this.organizeNodes)
    const listTablesChecked = this.getAllCheckedNodes(this.listTablesNodes)
    this.childEvent.emit({
      organizeChecked,
      listTablesChecked,
    })
  }

  generateUUID(): string {
    let d = new Date().getTime()
    if (window.performance && typeof window.performance.now === 'function') {
      d += performance.now() // Thêm độ chính xác của thời gian
    }
    const uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(
      /[xy]/g,
      (c) => {
        const r = (d + Math.random() * 16) % 16 | 0
        d = Math.floor(d / 16)
        return (c === 'x' ? r : (r & 0x3) | 0x8).toString(16)
      },
    )
    return uuid
  }
  updateTemplateData(): void {
    const selectedOrganize = this.treeOrganize.getCheckedNodeList();
    const selectedListTables = this.getAllCheckedNodes(
      this.treeListTables.getTreeNodes()
    );
    console.log("selectedListTables", selectedListTables);

    if (selectedOrganize.length === 0) {
      this.message.info('Vui lòng chọn ít nhất một đơn vị');
      return;
    }
    const templateCode = this.temDetail.code;
    let allUpdates: any[] = [];
    const newlySelectedOrgs = selectedOrganize.filter(org => !this.previouslyCheckedOrgs.includes(org.key));
    if (newlySelectedOrgs.length > 0) {
      for (const org of newlySelectedOrgs) {
        if (selectedListTables.length === 0) {
          allUpdates.push({
            isActive: true,
            code: this.generateUUID(),
            orgCode: org.key,
            templateCode: templateCode,
            listTablesCode: null
          });
        } else {
          const orgUpdates = selectedListTables.map(listTables => ({
            isActive: true,
            code: this.generateUUID(),
            orgCode: org.key,
            templateCode: templateCode,
            listTablesCode: listTables.origin['code']
          }));
          allUpdates.push(...orgUpdates);
        }
      }
    } else {
      for (const org of selectedOrganize) {
        if (selectedListTables.length === 0) {
          // Nếu không có listTables nào được chọn, thêm một bản ghi với listTablesCode là null
          allUpdates.push({
            isActive: true,
            code: this.generateUUID(),
            orgCode: org.key,
            templateCode: templateCode,
            listTablesCode: null
          });
        } else {
          // Create updates for each selected list table
          const orgUpdates = selectedListTables.map(listTables => ({
            isActive: true,
            code: this.generateUUID(),
            orgCode: org.key,
            templateCode: templateCode,
            listTablesCode: listTables.origin['code']
          }));
          allUpdates.push(...orgUpdates);
        }
      }
    }
    // for (const org of selectedOrganize) {
    //   // Kiểm tra xem tổ chức đã tồn tại trong previouslyCheckedOrgs chưa
    //   const isNewlySelected = !this.previouslyCheckedOrgs.includes(org.key);

    //   // Nếu là tổ chức mới hoặc đã tồn tại
    //   if (isNewlySelected || this.previouslyCheckedOrgs.includes(org.key)) {
    //     if (selectedListTables.length === 0) {
    //       // Nếu không có listTables nào được chọn, thêm một bản ghi với listTablesCode là null
    //       allUpdates.push({
    //         isActive: true,
    //         code: this.generateUUID(),
    //         orgCode: org.key,
    //         templateCode: templateCode,
    //         listTablesCode: null
    //       });
    //     } else {
    //       // Create updates for each selected list table
    //       const orgUpdates = selectedListTables.map(listTables => ({
    //         isActive: true,
    //         code: this.generateUUID(),
    //         orgCode: org.key,
    //         templateCode: templateCode,
    //         listTablesCode: listTables.origin['code']
    //       }));
    //       allUpdates.push(...orgUpdates);
    //     }
    //   }
    // }

    console.log("allUpdates before", allUpdates);

    // Chỉ gọi API nếu có dữ liệu cần cập nhật
    if (allUpdates.length > 0) {
      this.templateListTablesDataService.updateTemplateListTablesData(allUpdates).subscribe({
        next: (response) => {
          this.allCheckedNodes = new Set(
            allUpdates.map(item => item.listTablesCode).filter(code => code !== null)
          );
          this.updateLocalData(Array.from(this.allCheckedNodes));

          this.previouslyCheckedOrgs = selectedOrganize.map(org => org.key);
          this.newlyCheckedOrg = null;

          console.log("allUpdates response", allUpdates);
          this.sendDataToParent();
          this.dataUpdated.emit(this.temDetail.code);
        },
        error: (error) => {
          this.message.error('Cập nhật thất bại: ' + error.message);
          console.log(error);
        }
      });
    } else {
      this.message.info('Không có thay đổi để cập nhật');
    }
  }
  onTitleClick(node: any) {
    if (node.isLeaf) {
      node.isChecked = true
    } else {
      node.isChecked = false
    }
    // Gọi hàm xử lý thay đổi checkbox nếu cần
  }
  toggleChildrenChecked(node: NzTreeNode, checked: boolean): void {
    if (node.children) {
      node.children.forEach((child) => {
        child.isChecked = checked
        this.toggleChildrenChecked(child, checked)
      })
    }
  }

  searchListTables(searchValueListTables: string) {
    const filterNode = (node: NzTreeNodeOptions): boolean => {
      if (
        node.title.toLowerCase().includes(searchValueListTables.toLowerCase())
      ) {
        return true
      } else if (node.children) {
        node.children = node.children.filter((child) => filterNode(child))
        return node.children.length > 0
      }
      return false
    }

    if (!searchValueListTables) {
      this.listTablesNodes = [...this.originalListTablesNodes]
    } else {
      this.listTablesNodes = this.originalListTablesNodes
        .map((node) => ({ ...node }))
        .filter((node) => filterNode(node))
    }
  }

  searchOrganize(searchValue: string) {
    const filterNode = (node: NzTreeNodeOptions): boolean => {
      if (node.title.toLowerCase().includes(searchValue.toLowerCase())) {
        return true
      } else if (node.children) {
        node.children = node.children.filter((child) => filterNode(child))
        return node.children.length > 0
      }
      return false
    }

    if (!searchValue) {
      this.organizeNodes = [...this.originalOrganizeNodes]
    } else {
      this.organizeNodes = this.originalOrganizeNodes
        .map((node) => ({ ...node }))
        .filter((node) => filterNode(node))
    }
  }
  preview() {
    const templateCode = this.temDetail.code
    this.templateListTablesDataService
      .searchTemplateListTablesData({ KeyWord: templateCode })
      .subscribe({
        next: (result) => {
          if (result.data && result.data.length > 0) {
            this.router.navigate(
              [
                '/master-data/template-list-tables-preview',
                this.temDetail.code,
              ],
              { state: { templateListData: result.data } },
            )
          }
        },
        error: (error) => console.error(error),
      })
  }
  exportExcel() {
    this.loading = true
    const templateCode = this.temDetail.code
    return this.templateListTablesDataService
      .exportExcelDataTemplateListTablesData(templateCode)
      .subscribe((result: Blob) => {
        const blob = new Blob([result], {
          type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        })
        const url = window.URL.createObjectURL(blob)
        var anchor = document.createElement('a')
        anchor.download = this.temDetail.name + '.xlsx'
        anchor.href = url
        this.loading = false
        anchor.click()
      })
  }
}
