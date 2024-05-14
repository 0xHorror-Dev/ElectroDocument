from folder_structure import FolderStructureGenerator

# List of folders to be ignored in the folder structure generation
folders_to_ignore = [
    "__pycache__",
    ".git",
    ".idea",
    "venv",
    "bin",
    "obj",
    "DismissedEmployee.docx",
    "OD-SR-051.docx",
    "OD-SR-051-1.docx",
    "Perevod.doc",
    "Perevod.docx",
    "POOSHERENIE.doc",
    "t-5.docx",
    "TrudDocx.docx",
    "Weekend.docx",
    "Weekend1.docx",
    "wwwroot"
    
]

# Generate the markdown representation of the folder structure
folder_structure_generator = FolderStructureGenerator(ignored_folders=folders_to_ignore)
folder_structure_md = folder_structure_generator.generate_folder_structure_md()

# Print the markdown representation of the folder structure
print(folder_structure_md)