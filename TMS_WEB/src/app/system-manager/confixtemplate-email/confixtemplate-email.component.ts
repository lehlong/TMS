import { ShareModule } from '../../shared/share-module/index'
import { Component, AfterViewInit, OnInit } from '@angular/core'
import { GlobalService } from '../../services/global.service'
import { ChangeDetectorRef } from '@angular/core'
import { CKEditorModule } from '@ckeditor/ckeditor5-angular'
import {
  ClassicEditor,
  AccessibilityHelp,
  Alignment,
  Autosave,
  Bold,
  Essentials,
  FontBackgroundColor,
  FontColor,
  FontFamily,
  FontSize,
  GeneralHtmlSupport,
  Heading,
  Indent,
  IndentBlock,
  Italic,
  Link,
  Paragraph,
  SelectAll,
  ShowBlocks,
  SourceEditing,
  SpecialCharacters,
  Subscript,
  Superscript,
  Table,
  TableCaption,
  TableCellProperties,
  TableColumnResize,
  TableProperties,
  TableToolbar,
  Underline,
  Undo,
  EditorConfig,
} from 'ckeditor5'

@Component({
  selector: 'confixtemplate-email',
  standalone: true,
  imports: [ShareModule, CKEditorModule],
  templateUrl: './confixtemplate-email.component.html',
  styleUrls: ['./confixtemplate-email.component.scss'], // Đã đổi thành 'styleUrls'
})
export class ConfixTemplateEmailComponent implements AfterViewInit, OnInit {
  public Editor = ClassicEditor
  public isLayoutReady = false
  public config: EditorConfig = {}

  constructor(
    private globalService: GlobalService,
    private changeDetector: ChangeDetectorRef,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Cấu hình Template Email',
        path: 'system-manager/confixtemplate-email',
      },
    ])
  }

  public ngAfterViewInit(): void {
    this.config = {
      toolbar: {
        items: [
          'undo',
          'redo',
          '|',
          'sourceEditing',
          'showBlocks',
          '|',
          'heading',
          '|',
          'fontSize',
          'fontFamily',
          'fontColor',
          'fontBackgroundColor',
          '|',
          'bold',
          'italic',
          'underline',
          'subscript',
          'superscript',
          '|',
          'specialCharacters',
          'link',
          'insertTable',
          '|',
          'alignment',
          '|',
          'outdent',
          'indent',
        ],
        shouldNotGroupWhenFull: false,
      },
      plugins: [
        AccessibilityHelp,
        Alignment,
        Autosave,
        Bold,
        Essentials,
        FontBackgroundColor,
        FontColor,
        FontFamily,
        FontSize,
        GeneralHtmlSupport,
        Heading,
        Indent,
        IndentBlock,
        Italic,
        Link,
        Paragraph,
        SelectAll,
        ShowBlocks,
        SourceEditing,
        SpecialCharacters,
        Subscript,
        Superscript,
        Table,
        TableCaption,
        TableCellProperties,
        TableColumnResize,
        TableProperties,
        TableToolbar,
        Underline,
        Undo,
      ],
      fontFamily: {
        supportAllValues: true,
      },
      fontSize: {
        options: [10, 12, 14, 'default', 18, 20, 22],
        supportAllValues: true,
      },
      heading: {
        options: [
          {
            model: 'paragraph',
            title: 'Paragraph',
            class: 'ck-heading_paragraph',
          },
          {
            model: 'heading1',
            view: 'h1',
            title: 'Heading 1',
            class: 'ck-heading_heading1',
          },
          {
            model: 'heading2',
            view: 'h2',
            title: 'Heading 2',
            class: 'ck-heading_heading2',
          },
          {
            model: 'heading3',
            view: 'h3',
            title: 'Heading 3',
            class: 'ck-heading_heading3',
          },
          {
            model: 'heading4',
            view: 'h4',
            title: 'Heading 4',
            class: 'ck-heading_heading4',
          },
          {
            model: 'heading5',
            view: 'h5',
            title: 'Heading 5',
            class: 'ck-heading_heading5',
          },
          {
            model: 'heading6',
            view: 'h6',
            title: 'Heading 6',
            class: 'ck-heading_heading6',
          },
        ],
      },
      htmlSupport: {
        allow: [
          {
            name: /^.*$/,
            styles: true,
            attributes: true,
            classes: true,
          },
        ],
      },
      initialData: '<p>Hello</p>',
      link: {
        addTargetToExternalLinks: true,
        defaultProtocol: 'https://',
        decorators: {
          toggleDownloadable: {
            mode: 'manual',
            label: 'Downloadable',
            attributes: {
              download: 'file',
            },
          },
        },
      },
      placeholder: 'Type or paste your content here!',
      table: {
        contentToolbar: [
          'tableColumn',
          'tableRow',
          'mergeTableCells',
          'tableProperties',
          'tableCellProperties',
        ],
      },
    }

    this.isLayoutReady = true
    this.changeDetector.detectChanges()
  }

  ngOnInit(): void {}
}
