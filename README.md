<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/othneildrew/Best-README-Template">
    <img src="https://github.com/jonalaniz/CDOWin/blob/master/CDOWin/Assets/Square150x150Logo.scale-200.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">CDO.Win</h3>

  <p>
  	<img src="https://img.shields.io/badge/Windows-11-0078D6?logo=windows&logoColor=white" />
  	<img src="https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white" />
  	<img src="https://img.shields.io/badge/.NET-11-512BD4?logo=dotnet&logoColor=white" />
  </p>
</div>

<!-- ABOUT THE PROJECT -->
## About The Project
<div align="center">
  <img src="/Images/screenshot.png" alt="Logo" width="480">
</div>

**CDO.Win** is a modern rewrite of a legacy MS Access based application designed to help organizations catalogue clients, track progress, and support workforce training and placement.

The project was created to address several limitations of the original system:
* Removing reliance on proprietary software.
* Providing a modern interface optimized for speed and usavility.
* Enable a true multi-user experience without file locking.
* Moving business logic to the server to keep the client lightweight and performant, even on lower-end devices.

**Who this is for:**
CDO.Win is built for professionals working with the Texas Workforce ecosystem, including:
* Counselors
* Job coaches
* Staff

*This repository contains the front-end only, our backedn [CDO.Vapor](link-to-repositry) is required for full functionality.*

### Built With

CDO.Win is built with the latest versions of .NET and C#, using WinUI 3 (Windows App SDK 2.0 Preview).  
The project depends on several NuGet packages and requires a package restore before building.

<!-- GETTING STARTED -->
## Getting Started

You can download the lastest stable build from the Releases section (coming soon), or build the project from source using the instructions above.

### Prerequisites

Before you can use CDO.Win, ensure you have the following:
* A valid API key from your CDO.Vapor isntance for authentication.
* The domain name or IP address of your CDO.Vapor server.

### Installation

1. Obtain CDO.Win by downloading a release or building from source.
2. On first launch, CDO.Win will prompt your for your CDO.Vapor server address and API key.
3. Enter the information and click Test Connection to verify connectivity.
4. If the connection fails:
   * Confirm the server address is correct.
   * Verify the API key is valid.
   * Try opeing the server address in a web browser to ensure the CDO.Vapor page loads.
5. Once the test succeeds, click Save. CDO.Win will restart and be ready to use.
   
<!-- USAGE EXAMPLES -->
## Usage

Coming soon...

<!-- ROADMAP -->
## Roadmap

CDO.Win is fully fuctional and running in a live beta.
Core features currently include:
* Entity creation, editing, and deletion.
* Filtering and search.
* Invoicing workflows.
* Reminders and task tracking.
* Client activity tracking.

While the system is production-capable, current development is focused on moderninzing workflows inherited from the origianl Microsoft Access implementation. Some compatibility features (such as Microsoft Office interop integrations) were intentionally included to ensure continuity with the legacy environment and are now being progressivly replaced.

### Short-Term Improvements

- [ ] Add User documentation.
- [ ] Add User accounts, roles, and permissions.
- [ ] Pagination support for large ListView datasets.
- [ ] Modern invoice export using OpenXML (replacing legacy Word Interop).
- [ ] Unit test coverage for core services and business logic.

### Long-Term Roadmap
* Offline AI assistance using OllamaSharp
  * Resume and document generation.
  * Fully local inference for privacy-focused deployments.    

See the [open issues](https://github.com/jonalaniz/CDOWin/issues) for a full list of proposed features (and known issues).

<!-- Security -->
## Security

We recognize that CDO.Win operates in a domain that requires handling sensitive personal information. Protecting this data is a core design priority.
* Personally identifiable information (PII), including Social Security Numbers and government IDs, is encrypted on the server using AES-256 encryption.
* All client-server communication requires SSL/TLS to ensure data is encrypted in transit.
* API Keys are stored using Windows Credential Manager.

CDO.Win and CDO.Vapor are designed for internal organizational use only and are not recommended for direct exposure to the public internet.

If you discover a potential security issue or vulnerability, please open an issue so it can be reviewed and addressed promptly.

<!-- CONTRIBUTING -->
## Contributing

Contributions are always welcome and appreciated. If you would like to help improve CDO.Win:

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request describint your changes.

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<!-- CONTACT -->
## Contact

Jon Alaniz - jon@alaniz.tech

## Support

If you would live to support the project and keep development going, consider sponsoring or buying me a coffee. 

Your support helps cover development time and continued improvements to CDO.Win. Thank you for helping keep the project moving forward!

<a href="https://www.buymeacoffee.com/jonalaniz" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/yellow_img.png" alt="Buy Me A Coffee" height="41" width="174"></a>
