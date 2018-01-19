<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Informations = New System.Windows.Forms.GroupBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.Informations.SuspendLayout()
        Me.SuspendLayout()
        '
        'Informations
        '
        Me.Informations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Informations.Controls.Add(Me.Label3)
        Me.Informations.Controls.Add(Me.Label2)
        Me.Informations.Controls.Add(Me.Label1)
        Me.Informations.Location = New System.Drawing.Point(10, 4)
        Me.Informations.Name = "Informations"
        Me.Informations.Size = New System.Drawing.Size(733, 78)
        Me.Informations.TabIndex = 0
        Me.Informations.TabStop = False
        Me.Informations.Text = "Informations"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(18, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(91, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "<TexAnim file>"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "<mesh number>"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "<world file>"
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Font = New System.Drawing.Font("Lucida Console", 9.25!)
        Me.TextBox1.Location = New System.Drawing.Point(12, 88)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(733, 295)
        Me.TextBox1.TabIndex = 2
        Me.TextBox1.Text = resources.GetString("TextBox1.Text")
        '
        'TextBox2
        '
        Me.TextBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox2.AutoCompleteCustomSource.AddRange(New String() {"load                                                {load latest world file}", "load last file                                    {load latest world file}", "load <world file>                            {load a world file}", "open                                                  { open = load }", "open last file", "open lastfile ", "open <world file>         ", "save                                     {save the world file (not really needed " & _
                        ") }", "save <world file>", "script <script path>", "explore                                     {get to world's directory}", "count               ", "count meshes                                {count meshes}", "count polys                                 {count polys in current mesh}", "count vex                                     {count vertices in current mesh}", "count vertices                               {count vertices in current mesh}", "count vx                                     {count vertices in current mesh}", "getByTex <tex number>                  {get all polys using tex number}", "getByTex <tex A~Z>                       {get all polys using tex character}", "select <mesh>                                {select a mesh : IMPORTANT!}", "getInfo                                              {get current mesh info}", "list                                              {list polys in this mesh}", "listall                                              {list all polys in the whole" & _
                        " world}", "cls                                                {clear screen}", "shade A R G B                                {shade with transparence, if A=0: tr" & _
                        "ansparent}", "shade 0 0 0 0                                   {trick to get transparent mesh}", "shade R G B                                {shade to R G B}", "expot                                     {export a .prm file out of the mesh, rv" & _
                        "center needed}", "export <prmfile>                   {export prm file}", "export each                            {export each mesh, eq to ""for each: export" & _
                        """}", "convert quads                            {convert Triangulars to Quads polys}", "backup                                          {make a backup}", "restore                                                   {restore latest backup}" & _
                        "", "restore first                                         {restore first backup}", "restore <backup number>                  {restore a backup}", "log                                                       {append a log file}", "about                                                  {about}", "decode                                                  {decode a .w file manuall" & _
                        "y}", "decode <path>          ", "encode                                                {encode the decoded world f" & _
                        "ile}", "encode <path>          ", "cd <directory>                                     {change directory}", "run <exe/bat>                                       {run an application}", "revolt                                                          {launches RV in w" & _
                        "indow dev mode (if possible)}", "for each: instruction                                 {for each mesh loop}", "for 0 1 9 2  : instruction                            {for mesh numbers: 0,1, 2, " & _
                        "9}", "for 18 1->9 20: instruction                        {for mesh numbers 18, 20, 1,2," & _
                        "3,4,5,6,7,8,9}", "envshade                                                      {get ENVshade list}" & _
                        "", "envshade list", "envshade i R G B                                         {re-envshade all the ava" & _
                        "ilable ENV color}", "envshade <index> R G B                              {index: the env index}", "flag dblsd texanim trans ttype env               {set type}", "type dblsd texanim trans ttype env              {set type}", "silent                                                                {silent mod" & _
                        "e}", "preview" & Global.Microsoft.VisualBasic.ChrW(9) & "                                               {previews current mesh}", "preview *" & Global.Microsoft.VisualBasic.ChrW(9) & "                                              {preview current world}", "preview world", "preview mesh        ", "fvpreview" & Global.Microsoft.VisualBasic.ChrW(9) & "                                              {force freevolts to previ" & _
                        "ew mesh}", "fvwpreview                                             {force freevolt to preview" & _
                        " world}", "verbose                                                            {verbose mode}" & _
                        "", "texanime <Frame list's path>                       {load a frame_list file}", "animate                                                    {animate current mesh}" & _
                        ""})
        Me.TextBox2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.TextBox2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.TextBox2.Location = New System.Drawing.Point(16, 389)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(727, 20)
        Me.TextBox2.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(648, 358)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(95, 25)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "&Cancel"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.AliceBlue
        Me.ClientSize = New System.Drawing.Size(753, 424)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Informations)
        Me.Font = New System.Drawing.Font("Consolas", 8.25!)
        Me.ForeColor = System.Drawing.Color.Navy
        Me.Name = "Form1"
        Me.Text = "W_Console"
        Me.Informations.ResumeLayout(False)
        Me.Informations.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Informations As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
